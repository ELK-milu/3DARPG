using StatePattern.StateSystem;

public static class StateMachineExtensions
{
	public static StateMachine At(this StateMachine stateMachine, IState from, IState to, IPredicate condition)
	{
		stateMachine.AddTransition(from, to, condition);
		return stateMachine;
	}
	public static StateMachine Any(this StateMachine stateMachine, IState to, IPredicate condition)
	{
		stateMachine.AddAnyTransition(to, condition);
		return stateMachine;
	}
}
