namespace StatePattern.StateSystem
{
	/// <summary>
	/// 管理状态的上下文基类
	/// </summary>
	public class Context
	{
		// 当前状态
		private State _state;
		public void SetState<T>(T state) where T:State
		{
			_state = state;
		}
		public State GetState()
		{
			return _state;
		}
		public void Requst()
		{
			_state?.Handle();
		}
	}
	/// <summary>
	/// 上下文管理状态生命周期基类
	/// </summary>
	public class ContextBehaviour : Context
	{
		// 当前持有状态
		private StateBehaviour _stateBehaviour;
		// 覆盖父类的方法
		public new void SetState<T>(T state) where T:StateBehaviour
		{
			_stateBehaviour = state;
		}
		public new StateBehaviour GetState()
		{
			return _stateBehaviour;
		}
		public new void Requst()
		{
			_stateBehaviour?.Handle();
		}
		// 几个用于状态生命周期调度的虚方法
		public virtual void ChangeState(StateBehaviour stateBehaviour)
		{
			_stateBehaviour.Exit();
			SetState(stateBehaviour);
			_stateBehaviour.Enter();
		}
		public virtual void Update()
		{
			_stateBehaviour.Update();
		}
		public virtual void NotifyStateEnter()
		{
			_stateBehaviour.Enter();
		}
		public virtual void NotifyStateExit()
		{
			_stateBehaviour.Exit();
		}
	}

}
