using Architecture.StateSystem;
using InputSystem;
using KBCore.Refs;
using StatePattern.CameraState;
using StatePattern.PlayerState;
using StatePattern.StateSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Game.EntitySystem
{
	[RequireComponent(typeof(GroundChecker))]
	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(Rigidbody))]
	public class PlayerController : ValidatedMonoBehaviour
	{
		[Header("References")]
		[SerializeField, Anywhere] private CameraSystem _cameraSystem;
		[SerializeField, Self] private Rigidbody _rigidbody;
		[SerializeField, Self] private Animator _animator;
		[SerializeField, Self] private GroundChecker _groundChecker;
		[SerializeField, Anywhere] private InputReader _input;
		
		[Header("Attributions")]
		[SerializeField] public CharacterStateData HeroStateData;
		[Header("Running Setting")]
		[SerializeField] private float _basicSpeed = 10f;
		[SerializeField] private float _rotationSpeed = 15f;
		[SerializeField] private float _smoothTime = 0f;
		[Header("Jump Setting")]
		[SerializeField] private float _jumpForce = 10f;
		[SerializeField] private float _jumpDuration = 0.5f;
		[SerializeField] private float _jumpCoolDown = 0.5f;
		[SerializeField] private float _jumpMaxHeight = 0.5f;
		[SerializeField] private float _gravityMultiply = 0.5f;
		[Header("Dash Setting")]
		[SerializeField] private float _dashAcceleration = 10f;
		[SerializeField, Anywhere] private StaminCircle _staminCircle;
		[Tooltip("当前加速的速度")]
		[SerializeField] private float _dashVelocity = 10f;
		[SerializeField] private float _maxVelocity = 10f;
		[SerializeField] private float _decadeMultiply = 2f;
		[Tooltip("力竭状态强制等待体力条回满")]
		[SerializeField] public bool DisableDash = false;

		
		
		// Const variables
		private const float ZERO_F = 0f;

		
		// Private variables
		private Transform _playerTrans;
		
		// 
		private float _currentSpeed = 0f;
		private float _velocity = 0f;
		private float _moveSpeed = 10f;

		private Vector3 _movement;
		
		private bool _isFirstPersonMode = true;
		
		private float _jumpVelocity = 0f;
		
		// Private DataStructs
		private List<Timer> _timers = new List<Timer>();
		private CountdownTimer _jumpTimer;
		private CountdownTimer _jumpCoolDownTimer;
		
		// 类荒野之息的体力条,体力由HeroStateData中的基本属性面板计算总时长，然后作倒计时
		private CountdownTimer _dashTimer;
		private CountdownTimer _dashResetTimer;
		
		public StateMachine StateMachine { get; private set; }

		
		// Animator parameters
		private static readonly int Speed = Animator.StringToHash("Speed");


		#region MonoBehaviour
		private void Awake()
		{
			SetVariable();
			SetTimers();
			SetStateMachine();
			SetStatusCondition();
		}

		private ExhuastedCondition _exhuastedCondition;
		/// <summary>
		/// 提前生成好异常，以便切换
		/// </summary>
		private void SetStatusCondition()
		{
			_exhuastedCondition = new ExhuastedCondition(_dashTimer.GetInitialTime(), this);
		}

		public void AddStatusCondition (CharacterStateCondition statusCondition)
		{
			HeroStateData.StatusConditions.Add(statusCondition);
		}

		
		public void RemoveStatusCondition (CharacterStateCondition statusCondition)
		{
			statusCondition.Dispose();
			HeroStateData.StatusConditions.Remove(statusCondition);
		}
		
		private void SetVariable()
		{
			HeroStateData.ResetAttribution();
			_playerTrans = transform;
			_rigidbody.freezeRotation = true;
			_rigidbody.ResetInertiaTensor();
		}

		private void SetStateMachine()
		{
			var locomotionState = new LocomotionState(this, _animator);
			var jumpState = new JumpState(this, _animator);
			StateMachine = new StateMachine(locomotionState);
			StateMachine.At(jumpState, locomotionState, new FuncPredicate(() => !_jumpTimer.IsRunning && _groundChecker.IsGrounded));
			StateMachine.At(locomotionState, jumpState, new FuncPredicate(() => _jumpTimer.IsRunning));

		}

		private void SetTimers()
		{
			_jumpTimer = new CountdownTimer(_jumpDuration);
			_jumpCoolDownTimer = new CountdownTimer(_jumpCoolDown);

			_dashTimer = new CountdownTimer(HeroStateData.CharacterAttribution.MaxStamina);
			_dashResetTimer = new CountdownTimer(HeroStateData.CharacterAttribution.MaxStamina);

			//_dashTimer = new CountdownTimer(HeroStateData.CharacterAttribution.Health * 0.2f + HeroStateData.CharacterAttribution.Agile * 0.8f);

			_jumpTimer.OnTimerStart += () => _jumpVelocity = _jumpForce;
			_jumpTimer.OnTimerStop += () => _jumpCoolDownTimer.Start();

			_dashTimer.OnTimerStart += () =>
			{
				Debug.Log("_dashTimer.start");
				_dashResetTimer.Stop();
				_dashVelocity = 0f;
			};
			_dashTimer.OnTimerStop += () =>
			{
				if (_dashTimer.Progress <= 0.1f)
				{
					Debug.Log("力竭，强制禁止跑步");
					AddStatusCondition(_exhuastedCondition);
				}
				// 开启精力回复计时
				_dashResetTimer.ResetNowTime((1 - _dashTimer.Progress) * _dashTimer.GetInitialTime(),true);
			};
			_dashTimer.OnTimer += () => SetStamina(_dashTimer.Progress);


			_dashResetTimer.OnTimer += () =>
			{
				if (DisableDash && _dashResetTimer.Progress <= 0.1f)
				{
					RemoveStatusCondition(_exhuastedCondition);
				}
				Debug.Log("_dashResetTimer.OnTime");
				SetStamina(1 - _dashResetTimer.Progress);
			};
			_dashResetTimer.OnTimerStop +=()=>_dashTimer.ResetNowTime(HeroStateData.CharacterAttribution.CurrentStamina);
			

			_timers = new(4) { _jumpTimer, _jumpCoolDownTimer,_dashTimer,_dashResetTimer};
		}

		private void OnEnable()
		{
			_cameraSystem.OnEnterCameraStateHandler += OnCameraMode;
			_input.OnJumpHandler += OnJump;
			_input.OnDashHandler += OnDash;
		}
		private void OnDisable()
		{
			_cameraSystem.OnEnterCameraStateHandler -= OnCameraMode;
			_input.OnJumpHandler -= OnJump;
			_input.OnDashHandler -= OnDash;
		}

		new private void OnValidate()
		{
			this.ValidateRefs();
		}

		// Update is called once per frame
		void Update()
		{
			if (StateMachine != null)
			{
				StateMachine.Update();
			}
			HeroStateData.Update();
			_movement = new Vector3(_input.Direction.x, 0f, _input.Direction.y);
			UpdateAnimator();
			HandleTimers();
		}


		/// <summary>
		/// 物理计算都在FixedUpdate
		/// </summary>
		private void FixedUpdate()
		{
			StateMachine.FidedUpdate();
		}
		#endregion

		#region 移动计算
		
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
		
		private void HandleTimers()
		{
			foreach (var timer in _timers)
			{
				timer.Tick(Time.deltaTime);
			}
		}

		private void OnJump(bool isJump)
		{
			Debug.Log($"isJump:{isJump},_jumpTimer.IsRunning:{_jumpTimer.IsRunning},_jumpCoolDownTimer.IsRunning{_jumpCoolDownTimer.IsRunning},_groundChecker.IsGrounded:{_groundChecker.IsGrounded}");
			// 按下跳跃键 && 在地面上 && 未进入跳跃冷却
			if (isJump && !_jumpTimer.IsRunning && !_jumpCoolDownTimer.IsRunning && _groundChecker.IsGrounded) 
			{
				Debug.Log("按下跳跃键");
				_jumpTimer.Resume();
			} 
			// 放开跳跃键 && 正在跳跃
			else if (!isJump && _jumpTimer.IsRunning) 
			{
				Debug.Log("放开跳跃键");
				_jumpTimer.Stop();
			}
		}

		/// <summary>
		/// 按键时间越长，跳跃高度越高
		/// </summary>
		public void OnJumpMovement() {
			Debug.Log("计算跳跃移动");
			// If not jumping and grounded, keep jump velocity at 0
			if (!_jumpTimer.IsRunning && _groundChecker.IsGrounded) {
				_jumpVelocity = ZERO_F;
				return;
			}
            
			if (!_jumpTimer.IsRunning)
			{
				// Gravity takes over
				_jumpVelocity += Physics.gravity.y * _gravityMultiply * Time.fixedDeltaTime;
			}
			else
			{
				_jumpVelocity = Mathf.Sqrt(2 * _jumpMaxHeight * Mathf.Abs(Physics.gravity.y));
			}
            
			// Apply velocity
			_rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpVelocity, _rigidbody.velocity.z);
		}

		public void OnMovement()
		{

			var inputDirection = (_movement).normalized;
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
				CalculateHorizontalMovement(adjustedDirection); // 移动角色
				SmoothSpeed(adjustedDirection.magnitude); // 调整速度
			}
			else
			{
				SmoothSpeed(ZERO_F); // 停止移动
				_rigidbody.velocity = new Vector3(ZERO_F, _rigidbody.velocity.y, ZERO_F);
			}
		}
		
		
		private void OnDash (bool isDash)
		{
			if(DisableDash) return;
			// 按下冲刺键 && 在地面上 && 未进入冲刺状态
			if (isDash && _groundChecker.IsGrounded && !_dashTimer.IsRunning)
			{
				Debug.Log("按下冲刺键");
				_dashTimer.Start();
			}
			// 松开冲刺键 && 处于冲刺状态
			else if(!isDash && _dashTimer.IsRunning)
			{
				Debug.Log("松开冲刺键");
				_dashTimer.Stop();
			}
		}

		public void OnDashMovement()
		{
			// 正在冲刺 && 不在地面上
			if (_dashTimer.IsRunning && !_groundChecker.IsGrounded)
			{
				return;
			}
			
			if (_dashTimer.IsRunning)
			{
				// 1/2 at^2
				_dashVelocity += 0.5f * _dashAcceleration * Time.fixedDeltaTime * Time.fixedDeltaTime;
				_dashVelocity = Mathf.Min(_maxVelocity, _dashVelocity);
			}
			else if(_dashVelocity > 0f)
			{
				// 减速时急刹
				_dashVelocity -= _decadeMultiply * 0.5f * _dashAcceleration * Time.fixedDeltaTime * Time.fixedDeltaTime;
				_dashVelocity = Mathf.Max(ZERO_F, _dashVelocity);
			}
		}


		#region Calculate
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
		private void CalculateHorizontalMovement (Vector3 adjustedDirection)
		{
			var velocity = adjustedDirection * (_moveSpeed * Time.deltaTime);
			_rigidbody.velocity = new Vector3(velocity.x, _rigidbody.velocity.y, velocity.z);
		}

		private void CalculateRotation (Vector3 adjustedDirection)
		{
			var targetRotation = Quaternion.LookRotation(adjustedDirection);
			_playerTrans.rotation = Quaternion.RotateTowards(_playerTrans.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
		}

		private void SmoothSpeed (float targetVelocity)
		{
			_moveSpeed = _basicSpeed + _dashVelocity;
			_currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetVelocity, ref _velocity, _smoothTime);
		}
		#endregion

		#endregion

		#region 属性计算
		private void SetStamina(float value)
		{
			HeroStateData.CharacterAttribution.CurrentStamina = value * HeroStateData.CharacterAttribution.MaxStamina;
			_staminCircle.SetContainFill(value);
		}
		
		#endregion

	}
}
