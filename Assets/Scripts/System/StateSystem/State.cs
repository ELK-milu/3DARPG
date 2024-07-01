using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
namespace StatePattern.StateSystem
{
	/// <summary>
	/// 状态抽象基类
	/// </summary>
	public abstract class State
	{
		public abstract void Handle();
	}

	/// <summary>
	/// 状态生命周期抽象基类
	/// </summary>
	public abstract class StateBehaviour:State
	{
		// 状态持有者
		protected ContextBehaviour Context;

		// 几个用于状态生命周期调度的抽象方法
		public abstract void Update();
		public abstract void Enter();
		public abstract void Exit();
	}
}
