using Architecture.StateSystem;
using Cinemachine;
using KBCore.Refs;
using StatePattern.CameraState;
using System;
using UnityEngine;

namespace InputSystem
{
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(CharacterController))]
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerController : ValidatedMonoBehaviour
	{
		[Header("References")]
		[SerializeField, Anywhere] private CameraSystem _cameraSystem;
		[SerializeField, Self] private CharacterController _characterController;
		[SerializeField, Self] private Rigidbody _rigidbody;
		[SerializeField, Self] private Animator _animator;
		[SerializeField, Anywhere]
		private CinemachineFreeLook _freeLookCam;
		[SerializeField, Anywhere]
		private InputReader _input;
		
		[Header("Attributions")]
		[SerializeField]
		private float _moveSpeed = 10f;
		[SerializeField]
		public CharacterStateData HeroStateData;
		[SerializeField]
		private float _rotationSpeed = 15f;
		[SerializeField]
		private float _smoothTime = 0f;

		private Transform _playerTrans;
		private float _currentSpeed = 0f;
		private float _velocity = 0f;
		private bool _isFirstPersonMode = true;

		private const float ZERO_F = 0f;
		
		// Animator parameters
		private static readonly int Speed = Animator.StringToHash("Speed");


		#region MonoBehaviour
		private void Awake()
		{
			_playerTrans = transform;
		}

		private void OnEnable()
		{
			_cameraSystem.OnEnterCameraStateHandler += OnCameraMode;
		}
		private void OnDisable()
		{
			_cameraSystem.OnEnterCameraStateHandler -= OnCameraMode;
		}

		private void OnValidate()
		{
			this.ValidateRefs();
		}

		// Update is called once per frame
		void Update()
		{
			OnMovement();
			UpdateAnimator();
		}
		#endregion

		#region 移动计算
		
		/// <summary>
		/// TODO: 需要重构的方法，将摄像机视角处理放于CameraState的Update中
		/// 移动计算
		/// </summary>
		/// <param name="cameraState"></param>
		public void OnCameraMode(Type cameraState)
		{
			if (cameraState == typeof(FirstPersonCameraState))
			{
				_isFirstPersonMode = true;
			}
			else if(cameraState == typeof(ThirdPersonCameraState))
			{
				_isFirstPersonMode = false;
			}
		}

		private void UpdateAnimator()
		{
			if (_animator == null) return;
			_animator.SetFloat(Speed, _currentSpeed);
		}
		

		private void OnMovement()
		{
			// 如何将下列的输入方向转化为角色当前朝向的方向
			var inputVector = new Vector3(_input.Direction.x, 0f, _input.Direction.y);
			var inputDirection = (inputVector).normalized;
			inputDirection = Vector3.ClampMagnitude(inputDirection, 1f); // 确保输入向量的长度不超过1
			// 根据视角模式选择计算方式
			Vector3 adjustedDirection = CalculateAdjustedDirection(inputDirection,_isFirstPersonMode);
			if (_isFirstPersonMode)
			{
				var cameraRotation = _cameraSystem.MainCamera.transform.rotation;

				_playerTrans.rotation = new Quaternion(0, cameraRotation.y,0, cameraRotation.w);
			}
			// 确保有有效的移动方向
			if (adjustedDirection.magnitude > ZERO_F)
			{
				if (!_isFirstPersonMode)
				{
					CalculateRotation(adjustedDirection); // 假设CalculateRotation()需要根据移动方向调整角色朝向
				}

				CalculateMoveMent(adjustedDirection); // 移动角色
				SmoothSpeed(adjustedDirection.magnitude); // 调整速度
			}
			else
			{
				SmoothSpeed(ZERO_F); // 停止移动
			}
		}
		
		private Vector3 CalculateAdjustedDirection(Vector3 inputDirection, bool isFirstPersonMode)
		{
			Vector3 adjustedDirection;
			if (isFirstPersonMode)
			{
				// 第一人称视角下，角色直接面向输入方向移动，无需根据摄像机旋转调整
				adjustedDirection = _playerTrans.forward * inputDirection.z + _playerTrans.right * inputDirection.x;
			}
			else
			{
				// 第三人称视角下，角色面向摄像机的朝向移动
				adjustedDirection = Quaternion.AngleAxis(_cameraSystem.MainCamera.transform.eulerAngles.y, Vector3.up) * inputDirection;
			}
			return adjustedDirection;
		}
		private void CalculateMoveMent (Vector3 adjustedDirection)
		{
			var moveMent = adjustedDirection * (_moveSpeed * Time.deltaTime);
			_characterController.Move(moveMent);
		}

		private void CalculateRotation (Vector3 adjustedDirection)
		{
			var targetRotation = Quaternion.LookRotation(adjustedDirection);
			_playerTrans.rotation = Quaternion.RotateTowards(_playerTrans.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
			_playerTrans.LookAt(_playerTrans.position + adjustedDirection);
		}

		private void SmoothSpeed (float targetVelocity)
		{
			_currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetVelocity, ref _velocity, _smoothTime);
		}
		#endregion

		#region 状态切换
		
		#endregion

	}
}
