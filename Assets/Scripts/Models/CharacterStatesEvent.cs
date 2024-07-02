using EventPattern.EventSystem;

namespace EventPattern.PlayerEvent
{

	public struct TestEvent : IEvent
	{
		
	}
	public struct CharacterStatesEvent:IEvent
	{
		public int HealthCost;
		public int ManaCost;
	}
}
