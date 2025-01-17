﻿using Game.EntitySystem;
using StatePattern.StateSystem;
using System.Collections.Generic;
using UnityEngine;

namespace StatePattern.PlayerState
{
	public abstract class BasePlayerState : IState
	{
		protected readonly PlayerController _playerController;
		protected readonly Animator _animator;

		protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
		protected static readonly int JumpHash = Animator.StringToHash("Jump");
		
		protected const float CROSS_FADE_DURATION = 0.1f;

		/// <summary>
		/// 存储了动画的哈希值和过渡时长的字典
		/// </summary>
		/// <param name="int">动画的哈希值</param>
		/// <param name="float">动画的过渡时长</param>
		protected static Dictionary<int, float> animHashDictionary = new Dictionary<int, float>();

		public BasePlayerState (PlayerController playerController, Animator animator)
		{
			_playerController = playerController;
			_animator = animator;
		}

		public abstract void OnEnter();

		public abstract void Update();

		public abstract void FixedUpdate();

		public abstract void OnExit();
	}
	public class LocomotionState : BasePlayerState
	{
		public LocomotionState (PlayerController playerController, Animator animator) : base(playerController, animator)
		{
		}

		public override void OnEnter()
		{
			_animator.CrossFade(LocomotionHash,CROSS_FADE_DURATION);
		}

		public override void Update()
		{
		}

		public override void FixedUpdate()
		{
			_playerController.OnMovement();
			_playerController.OnDashMovement();
		}

		public override void OnExit()
		{
			
		}
	}

	public class JumpState : BasePlayerState
	{
		public JumpState(PlayerController playerController, Animator animator) : base(playerController, animator) { }

		public override void OnEnter()
		{
			_animator.CrossFade(JumpHash, CROSS_FADE_DURATION);
		}

		public override void Update()
		{
		}

		public override void FixedUpdate()
		{
			_playerController.OnMovement();
			_playerController.OnJumpMovement();
		}

		public override void OnExit()
		{
		}
	}

}