using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
	[CreateAssetMenu(fileName = "InputReader",menuName = "ScriptableObjects/InputReader")]
	public class InputReader : ScriptableObject,PlayerInputActions.IPlayerActions,ISingleTon<PlayerInputActions>
	{
		/// <summary>
		/// 带参委托说明需要对Input的参数进行处理
		/// </summary>
		public event Action<Vector2> OnMoveHandler = delegate (Vector2 vector2) {  }; 
		public event Action<Vector2,bool> OnLookHandler = delegate (Vector2 vector2, bool b) {  };
		/// <summary>
		/// 无参委托只需在按键时触发事件即可
		/// </summary>
		public event Action OnAttackHandler = delegate {  };
		public event Action OnLockCameraHandler = delegate {  };
		public event Action<bool> OnJumpHandler = delegate {  };
		public event Action<bool> OnDashHandler = delegate {  };


		private static PlayerInputActions _playerInputActions;
		public object locker { get; set; }

		public PlayerInputActions SingleTon
		{
			get
			{
				if (_playerInputActions == null)
				{
					_playerInputActions = new PlayerInputActions();
					_playerInputActions.Player.SetCallbacks(this);
				}
				return _playerInputActions;
			}
		}

		public Vector3 Direction => SingleTon.Player.Move.ReadValue<Vector2>();

		public void ClearLook()
		{
			OnLookHandler = delegate (Vector2 vector2, bool b) {  };
		}
		
		private void OnEnable()
		{
			SingleTon.Enable();
		}

		private void OnDisable()
		{
			SingleTon.Disable();
		}

		public void OnMove (InputAction.CallbackContext context)
		{
			OnMoveHandler?.Invoke(context.ReadValue<Vector2>());
		}

		public void OnLook (InputAction.CallbackContext context)
		{
			OnLookHandler?.Invoke(context.ReadValue<Vector2>(),IsDeviceMouse(context));
		}

		private bool IsDeviceMouse (InputAction.CallbackContext context)
		{
			return context.control.device.name == "Mouse";
		}

		public void OnAttack (InputAction.CallbackContext context)
		{
			OnAttackHandler?.Invoke();
		}

		public void OnLockCamera (InputAction.CallbackContext context)
		{
			switch (context.phase)
			{
				case InputActionPhase.Started:
					OnLockCameraHandler?.Invoke();
					break;
			}
		}

		public void OnJump (InputAction.CallbackContext context)
		{
			switch (context.phase)
			{
				case InputActionPhase.Started:
					OnJumpHandler?.Invoke(true);
					break;
				case InputActionPhase.Canceled:
					OnJumpHandler?.Invoke(false);
					break;
			}
		}

		public void OnDash (InputAction.CallbackContext context)
		{
			switch (context.phase)
			{
				case  InputActionPhase.Started:
					OnDashHandler?.Invoke(true);
					break;
				case  InputActionPhase.Performed:
					OnDashHandler?.Invoke(true);
					break;
				case InputActionPhase.Canceled:
					OnDashHandler?.Invoke(false);
					break;
			}
		}
	}
}
