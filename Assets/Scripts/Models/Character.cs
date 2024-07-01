using StatePattern.StateSystem;
using UnityEngine;
using UnityEngine.UI;

namespace StatePattern.CharacterBase
{

	#region 基类定义
	
	/// <summary>
	/// 角色状态基类
	/// </summary>
	public abstract class CharacterState : StateBehaviour { }
	/// <summary>
	/// 角色状态上下文基类
	/// </summary>
	public class CharacterContext : ContextBehaviour { }
	/// <summary>
	/// 角色基类
	/// </summary>
	public class Character : MonoBehaviour
	{
		private CharacterContext _context;
		public CharacterContext Context => _context;
		public Button StateChangeBtn;

		private void Start()
		{
			var riginState = new IdleState();
			_context = new CharacterContext();
			_context.SetState(riginState);
			var newState = new MoveState();
			StateChangeBtn.onClick.AddListener(() => { ChangeState(newState); });
		}

		private void Update()
		{
			_context.Update();
		}

		public void ChangeState(CharacterState characterState)
		{
			_context.ChangeState(characterState);
		}
	}
	#endregion

	
	/// <summary>
	/// 角色状态类IdleState
	/// </summary>
	public class IdleState : CharacterState
	{
		public override void Update()
		{
			Debug.Log("处于IdleState");
		}
		public override void Enter()
		{
			Debug.Log("进入IdleState");
		}
		public override void Exit()
		{
			Debug.Log("退出IdleState");
		}
		public override void Handle()
		{
			Debug.Log("IdleState下执行事件");
		}
	}
	
	/// <summary>
	/// 角色状态类MoveState
	/// </summary>
	public class MoveState : CharacterState
	{
		public override void Update()
		{
			Debug.Log("处于MoveState");
		}
		public override void Enter()
		{
			Debug.Log("进入MoveState");
		}
		public override void Exit()
		{
			Debug.Log("退出MoveState");
		}
		public override void Handle()
		{
			Debug.Log("MoveState下执行事件");
		}
	}

}