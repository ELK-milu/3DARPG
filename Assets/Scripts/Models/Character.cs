using StatePattern.StateSystem;
using UnityEngine;
using UnityEngine.UI;

namespace StatePattern.CharBase
{

	#region 基类定义
	
	/// <summary>
	/// 角色状态基类
	/// </summary>
	public abstract class CharState : StateBehaviour { }
	/// <summary>
	/// 角色状态上下文基类
	/// </summary>
	public class CharContext : ContextBehaviour { }
	/// <summary>
	/// 角色基类
	/// </summary>
	public class Character : MonoBehaviour
	{
		private CharContext _context;
		public CharContext Context => _context;
		public Button StateChangeBtn;

		private void Start()
		{
			var riginState = new IdleState();
			_context = new CharContext();
			_context.SetState(riginState);
			var newState = new MoveState();
			StateChangeBtn.onClick.AddListener(() => { ChangeState(newState); });
		}

		private void Update()
		{
			_context.Update();
		}

		public void ChangeState(CharState charState)
		{
			_context.ChangeState(charState);
		}
	}
	#endregion

	
	/// <summary>
	/// 角色状态类IdleState
	/// </summary>
	public class IdleState : CharState
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
	public class MoveState : CharState
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