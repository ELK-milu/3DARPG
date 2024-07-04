namespace StatePattern.StateSystem
{
	public interface ITransition
	{
		IState To { get; }
		IPredicate Condition { get; }
	}


}
