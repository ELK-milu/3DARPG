using InputSystem;
using StatePattern.PlayerState;
using UnityEngine;

namespace StatePattern.StateSystem
{
	public class LocalmotionState : BasePlayerState
	{
		public LocalmotionState (PlayerController playerController, Animator animator) : base(playerController, animator)
		{
		}

		public override void OnEnter()
		{
			_animator.CrossFade(LocomotionHash,10f);
		}

		public override void Update()
		{
		}

		public override void FixedUpdate()
		{
		}

		public override void OnExit()
		{
			
		}
	}
}
