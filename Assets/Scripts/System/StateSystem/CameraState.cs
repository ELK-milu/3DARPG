﻿using Game.EntitySystem;
using StatePattern.StateSystem;
using System;

namespace StatePattern.CameraState
{
	public class CameraState : IState
	{
		protected CameraSystem _cameraSystem;
		protected PlayerController _playerController;

		public CameraState (CameraSystem cameraSystem,PlayerController playerController)
		{
			_cameraSystem = cameraSystem;
			_playerController = playerController;
		}
		
		public virtual void OnEnter()
		{
		}

		public virtual void Update()
		{
		}

		public virtual void FixedUpdate()
		{
		}

		public virtual void OnExit()
		{
		}

		public Type GetStateType()
		{
			return this.GetType();
		}
	}

	public class ThirdPersonCameraState : CameraState
	{
		public ThirdPersonCameraState (CameraSystem cameraSystem,PlayerController playerController) : base(cameraSystem,playerController)
		{
		}

		public override void OnEnter()
		{
			_cameraSystem.ThirdPersonCamera.Priority = 10;
			string[] culls = {"Health"};
			_cameraSystem.SetCullingMask(culls);
			_cameraSystem.OnEnterCameraPerson();
		}

		public override void Update()
		{
		}

		public override void FixedUpdate()
		{
		}

		public override void OnExit()
		{
			_cameraSystem.ThirdPersonCamera.Priority = 0;
			_cameraSystem.SetAllMask();
		}
	}

	public class FirstPersonCameraState : CameraState
	{
		public FirstPersonCameraState (CameraSystem cameraSystem,PlayerController playerController) : base(cameraSystem,playerController)
		{
		}

		public override void OnEnter()
		{
			_cameraSystem.FirstPersonCamera.Priority = 10;
			string[] culls = {"Head","Health"};
			_cameraSystem.SetCullingMask(culls);
			_cameraSystem.OnEnterCameraPerson();
		}

		public override void Update()
		{
		}

		public override void FixedUpdate()
		{
		}

		public override void OnExit()
		{
			_cameraSystem.FirstPersonCamera.Priority = 0;
			_cameraSystem.SetAllMask();
		}
	}
}
