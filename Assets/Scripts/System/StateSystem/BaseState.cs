using InputSystem;
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
		
		public virtual void OnEnter()
		{
			// noop
		}

		public virtual void Update()
		{
			// noop
		}

		public virtual void FixedUpdate()
		{
			// noop
		}

		public virtual void OnExit()
		{
			// noop
		}
	}
}