using System;
using System.Collections.Generic;

namespace StatePattern.StateSystem
{
	public class StateMachine
	{
		public StateNode CurrentState { get; private set; }
		// 存储状态节点
		Dictionary<Type,StateNode> _stateNodes = new Dictionary<Type, StateNode>();
		// 存储需要转换状态的哈希表
		HashSet<ITransition> _anyTransitions = new HashSet<ITransition>();

		public StateMachine (IState state)
		{
			SetState(state);
		}
		/// <summary>
		/// 获取状态的函数在Update中执行
		/// </summary>
		public void Update()
		{
			var transition = GetTransition();
			if (transition != null)
			{
				ChangeState(transition.To);
			}
			CurrentState.State?.Update();
		}

		public void FidedUpdate()
		{
			CurrentState.State?.FixedUpdate();
		}

		public void SetState (IState state)
		{
            CurrentState = GetOrAddNode(state);
            CurrentState.State?.OnEnter();
		}
		
		public void ChangeState (IState state)
		{
			if (state == CurrentState.State)	return;
			CurrentState.State?.OnExit();
			CurrentState = _stateNodes[state.GetType()];
			CurrentState.State?.OnEnter();
		}

		/// <summary>
		///  获取转换状态,以StateMachine中HashSet存储的状态优先,若触发则任意时刻都能转换,而StateNode的状态转换仅限内部存储的转换
		/// </summary>
		/// <returns></returns>
		ITransition GetTransition()
		{
			ITransition thisTransition = null; 
			foreach (var transition in _anyTransitions)
			{
				if (transition.Condition.Evaluate())
				{
					thisTransition = transition;
					return thisTransition;
				}
			}


			foreach (var transition in CurrentState.Transitions)
			{
				if (transition.Condition.Evaluate())
				{
					thisTransition = transition;
					return thisTransition;
				}
			}
			return thisTransition;
		}

		public void AddTransition (IState from,IState to, IPredicate condition)
		{
			GetOrAddNode(from).AddTransition(GetOrAddNode(to).State,condition);			
		}

		public void AddAnyTransition (IState to, IPredicate condition)
		{
			_anyTransitions.Add(new Transition(GetOrAddNode(to).State,condition));
		}

		private StateNode GetOrAddNode (IState state)
		{
			if (!_stateNodes.TryGetValue(state.GetType(),out var stateNode))
			{
				stateNode = new StateNode(state);
				_stateNodes.Add(state.GetType(),stateNode);
			}
			return stateNode;
		}

		/// <summary>
		/// 状态节点类包含了状态本身以及转换的其他状态,对比状态机,IState是节点，而ITransition代表了不同状态节点间转换的路径
		/// </summary>
		public class StateNode
		{
			public IState State { get; }
			public HashSet<ITransition> Transitions { get; }

			public StateNode(IState state)
			{
				State = state;
				Transitions = new HashSet<ITransition>();
			}
			public void AddTransition(ITransition transition)
			{
				Transitions.Add(transition);
			}

			public void AddTransition (IState to, IPredicate condition)
			{
				Transitions.Add(new Transition(to, condition));
			}
        
		}
	}

}
