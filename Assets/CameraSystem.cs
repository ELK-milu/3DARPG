using Cinemachine;
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
	[SerializeField,Child]public Camera MainCamera;
	[SerializeField,Child]public CinemachineFreeLook ThirdPersonCamera;
	[SerializeField,Child]public CinemachineVirtualCamera FirstPersonCamera;
	public StateMachine StateMachine { get; private set; }
	[Header("Settings")] 
	[SerializeField, Range(0.5f, 3f)] float speedMultiplier = 1f;
        
	public event Action<Type> OnEnterCameraStateHandler = delegate {  };
	
	bool isRMBPressed;
	bool cameraMovementLock;
	private void Awake()
	{
		var FreeLookState = new ThirdPersonCameraState(this);
		var FirstPersonState = new FirstPersonCameraState(this);
		StateMachine = new StateMachine(FirstPersonState);
		At(FreeLookState, FirstPersonState, new FuncPredicate(
			() => Input.GetKeyDown(KeyCode.F3)&&
			      StateMachine.CurrentState.State.GetType() == FreeLookState.GetType()
		));
		At(FirstPersonState, FreeLookState, new FuncPredicate(
			() => Input.GetKeyDown(KeyCode.F3)&&
			      StateMachine.CurrentState.State.GetType() == FirstPersonState.GetType()
		));
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
		_input.OnEnableMouseControlCameraHandler += OnEnableMouseControlCamera;
		_input.OnDisableMouseControlCameraHandler += OnDisableMouseControlCamera;
	}
        
	void OnDisable() {
		_input.OnLookHandler -= OnThirdPersonLook;
		_input.OnEnableMouseControlCameraHandler -= OnEnableMouseControlCamera;
		_input.OnDisableMouseControlCameraHandler -= OnDisableMouseControlCamera;
	}
	#endregion

	#region 相机事件
	public void OnEnterCameraPerson()
	{
		OnEnterCameraStateHandler.Invoke(StateMachine.CurrentState.State.GetType());
	}

	#endregion

	
	void At (IState from, IState to, IPredicate condition)
	{
		StateMachine.AddTransition(from, to, condition);
	}

	void Any (IState to, IPredicate condition)
	{
		StateMachine.AddAnyTransition(to, condition);
	}

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
				Debug.Log($"剔除: {layerName}. 层级{layerIndex}.");
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
            
		if (isDeviceMouse && !isRMBPressed) return;

		// If the device is mouse use fixedDeltaTime, otherwise use deltaTime
		float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;
            
		// Set the camera axis values
		ThirdPersonCamera.m_XAxis.m_InputAxisValue = cameraMovement.x * speedMultiplier * deviceMultiplier;
		ThirdPersonCamera.m_YAxis.m_InputAxisValue = cameraMovement.y * speedMultiplier * deviceMultiplier;
	}

	public void OnFirstPersonLook(Vector2 cameraMovement, bool isDeviceMouse) 
	{
		if (cameraMovementLock) return;
            
		if (isDeviceMouse && !isRMBPressed) return;

		// If the device is mouse use fixedDeltaTime, otherwise use deltaTime
		float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;
            
		// Set the camera axis values
		FirstPersonCamera.transform.localRotation = Quaternion.Euler(cameraMovement.x * speedMultiplier * deviceMultiplier, 0f, 0f);
	}
	
	void OnEnableMouseControlCamera() {
		isRMBPressed = true;
            
		// Lock the cursor to the center of the screen and hide it
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
            
		StartCoroutine(DisableMouseForFrame());
	}

	void OnDisableMouseControlCamera() {
		isRMBPressed = false;
            
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
