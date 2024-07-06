using Cinemachine;
using Game.EntitySystem;
using InputSystem;
using KBCore.Refs;
using StatePattern.CameraState;
using StatePattern.StateSystem;
using System;
using System.Collections;
using UnityEngine;

public class CameraSystem : ValidatedMonoBehaviour
{
	[SerializeField, Anywhere] InputReader _input;
	[SerializeField, Anywhere] PlayerController _player;
	[SerializeField,Child]public Camera MainCamera;
	[SerializeField,Child]public CinemachineFreeLook ThirdPersonCamera;
	[SerializeField,Child]public CinemachineVirtualCamera FirstPersonCamera;
	public StateMachine StateMachine { get; private set; }
	[Header("Settings")] 
	[SerializeField, Range(0.5f, 3f)] float speedMultiplier = 1f;
        
	public event Action<Type> OnEnterCameraStateHandler = delegate (Type type) { };
	
	bool isCameraLocked = false;
	bool cameraMovementLock;
	private void Awake()
	{
		var _playerTrans = _player.transform;
		ThirdPersonCamera.Follow = _playerTrans;
		ThirdPersonCamera.LookAt = _playerTrans;
		ThirdPersonCamera.OnTargetObjectWarped(_playerTrans,_playerTrans.position - ThirdPersonCamera.transform.position - Vector3.forward);
		
		var FreeLookState = new ThirdPersonCameraState(this,_player);
		var FirstPersonState = new FirstPersonCameraState(this,_player);
		StateMachine = new StateMachine(FirstPersonState);
		StateMachine.At(FreeLookState, FirstPersonState, new FuncPredicate(
			() => Input.GetKeyDown(KeyCode.F3)&&
			      StateMachine.CurrentState.State.GetType() == FreeLookState.GetType()
		));
		StateMachine.At(FirstPersonState, FreeLookState, new FuncPredicate(
			() => Input.GetKeyDown(KeyCode.F3)&&
			      StateMachine.CurrentState.State.GetType() == FirstPersonState.GetType()
		));
		_player.OnCameraMode(StateMachine.CurrentState.State.GetType());
		
	}

	#region MonoBehaviour
	private void Update()
	{
		if (StateMachine != null)
		{
			StateMachine.Update();
		}
	}

	void OnEnable()
	{
		_input.OnLookHandler += OnThirdPersonLook;
		_input.OnLockCameraHandler += OnLockCamera;
	}
        
	void OnDisable() {
		_input.OnLookHandler -= OnThirdPersonLook;
		_input.OnLockCameraHandler -= OnLockCamera;
	}
	#endregion

	#region 相机事件
	public void OnEnterCameraPerson()
	{
		OnEnterCameraStateHandler?.Invoke(StateMachine?.CurrentState.State.GetType());
	}

	#endregion


	#region settings
	public void SetCullingMask(string[] culls)
	{
		// 输入验证：检查数组是否为空
		if (culls == null || culls.Length == 0)
		{
			Debug.LogWarning("SetCullingMask received an empty or null array. No changes will be made to the culling mask.");
			return;
		}

		// 初始化一个用于存储有效层遮罩的int型变量
		int validMask = -1;

		// 遍历数组，将有效的层名转换为遮罩值
		foreach (string layerName in culls)
		{
			// 检查层名是否有效
			int layerIndex = LayerMask.NameToLayer(layerName);
			if (layerIndex >= 0 && layerIndex < 32) // 确保层索引在有效范围内
			{
				validMask &= ~(1 << layerIndex);
			}
			else
			{
				Debug.LogError($"Invalid layer name: {layerName}. This layer will be ignored.");
			}
		}

		// 如果没有有效的层，记录警告并返回
		if (validMask == -1)
		{
			Debug.LogWarning("SetCullingMask did not find any valid layer names. No changes will be made to the culling mask.");
			return;
		}

		// 安全地更新MainCamera的cullingMask
		try
		{
			MainCamera.cullingMask = validMask;
		}
		catch (Exception ex)
		{
			// 异常处理：记录异常但不阻止程序执行
			Debug.LogError($"An exception occurred while setting the culling mask: {ex}");
		}
	}

	public void SetMask(string[] masks)
	{
		// 输入验证：检查数组是否为空
		if (masks == null || masks.Length == 0)
		{
			Debug.LogWarning("SetCullingMask received an empty or null array. No changes will be made to the culling mask.");
			return;
		}

		// 初始化一个用于存储有效层遮罩的int型变量
		int validMask = 0;

		// 遍历数组，将有效的层名转换为遮罩值
		foreach (string layerName in masks)
		{
			// 检查层名是否有效
			int layerIndex = LayerMask.NameToLayer(layerName);
			if (layerIndex >= 0 && layerIndex < 32) // 确保层索引在有效范围内
			{
				validMask |= (1 << layerIndex);
			}
			else
			{
				Debug.LogError($"Invalid layer name: {layerName}. This layer will be ignored.");
			}
		}

		// 如果没有有效的层，记录警告并返回
		if (validMask == 0)
		{
			Debug.LogWarning("SetMask did not find any valid layer names. No changes will be made to the culling mask.");
			return;
		}

		// 安全地更新MainCamera的cullingMask
		try
		{
			MainCamera.cullingMask = validMask;
		}
		catch (Exception ex)
		{
			// 异常处理：记录异常但不阻止程序执行
			Debug.LogError($"An exception occurred while setting the culling mask: {ex}");
		}
	}

	public void SetAllMask()
	{
		MainCamera.cullingMask = -1;
	}
	public void OnThirdPersonLook(Vector2 cameraMovement, bool isDeviceMouse) 
	{
		if (cameraMovementLock) return;
            
		if (isDeviceMouse && !isCameraLocked) return;

		// If the device is mouse use fixedDeltaTime, otherwise use deltaTime
		float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;
            
		// Set the camera axis values
		ThirdPersonCamera.m_XAxis.m_InputAxisValue = cameraMovement.x * speedMultiplier * deviceMultiplier;
		ThirdPersonCamera.m_YAxis.m_InputAxisValue = cameraMovement.y * speedMultiplier * deviceMultiplier;
	}

	public void OnFirstPersonLook(Vector2 cameraMovement, bool isDeviceMouse) 
	{
		if (cameraMovementLock) return;
            
		if (isDeviceMouse && !isCameraLocked) return;

		// If the device is mouse use fixedDeltaTime, otherwise use deltaTime
		float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;
            
		// Set the camera axis values
		FirstPersonCamera.transform.localRotation = Quaternion.Euler(cameraMovement.x * speedMultiplier * deviceMultiplier, 0f, 0f);
	}
	
	void OnLockCamera()
	{
		if (isCameraLocked)
		{
			isCameraLocked = false;
			cameraMovementLock = false;
			// Unlock the cursor and make it visible
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
            
			// Reset the camera axis to prevent jumping when re-enabling mouse control
			ThirdPersonCamera.m_XAxis.m_InputAxisValue = 0f;
			ThirdPersonCamera.m_YAxis.m_InputAxisValue = 0f;
		}
		else
		{
			isCameraLocked = true;
			cameraMovementLock = true;
			// Lock the cursor to the center of the screen and hide it
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	void OnDisableMouseControlCamera() {
		isCameraLocked = false;
            
		// Unlock the cursor and make it visible
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
            
		// Reset the camera axis to prevent jumping when re-enabling mouse control
		ThirdPersonCamera.m_XAxis.m_InputAxisValue = 0f;
		ThirdPersonCamera.m_YAxis.m_InputAxisValue = 0f;
	}
	IEnumerator DisableMouseForFrame() {
		cameraMovementLock = true;
		yield return new WaitForEndOfFrame();
		cameraMovementLock = false;
	}
	#endregion


}