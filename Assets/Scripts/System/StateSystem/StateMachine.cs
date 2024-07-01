using System.Collections.Generic;

namespace StatePattern.StateSystem
{
	public class NullState : StateBehaviour
	{
		public override void Handle()
		{
			throw new System.NotImplementedException();
		}

		public override void Update()
		{
			throw new System.NotImplementedException();
		}

		public override void Enter()
		{
			throw new System.NotImplementedException();
		}

		public override void Exit()
		{
			throw new System.NotImplementedException();
		}
	}
	public class StateMachine
	{
		private ContextBehaviour _contextBehaviour;
		public ContextBehaviour ContextBehaviour => _contextBehaviour;

		private NullState _nullState = new NullState();
		private StateBehaviour _prevState= new NullState();
		
		public StateMachine (ContextBehaviour contextBehaviour)
		{
			_contextBehaviour = contextBehaviour;
		}
		public StateMachine (ContextBehaviour contextBehaviour,StateBehaviour riginState)
		{
			_contextBehaviour = contextBehaviour;
			_contextBehaviour.SetState(riginState);
		}
		
		private Queue<StateBehaviour> _stateQueue = new Queue<StateBehaviour>();

		public void StateEnQueue(StateBehaviour stateBehaviour)
		{
			_stateQueue.Enqueue(stateBehaviour);
		}

		public StateBehaviour StateDeQueue()
		{
			if (_stateQueue.Count > 0)
			{
				return _stateQueue.Dequeue();
			}
			else
			{
				return _nullState;
			}
		}

		public void Update()
		{
			_contextBehaviour.Update();
		}

		public void NextState()
		{
			_prevState = _contextBehaviour.GetState();
			_contextBehaviour.ChangeState(StateDeQueue());
		}
		
		public void PrevState()
		{
			_contextBehaviour.ChangeState(_prevState);
		}
	}
}
