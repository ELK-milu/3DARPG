using EventPattern.EventSystem;

namespace EventPattern.AbilityEvent
{
	public class AbilityEvent : IEvent
	{
		public float Duration;
		public int index;

		public AbilityEvent(float duration)
		{
			Duration = duration;
		}
	}
}
