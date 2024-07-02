using EventPattern.EventSystem;
using EventPattern.PlayerEvent;
using UnityEngine;

namespace Character.Hero
{
	public class HeroData:MonoBehaviour
	{
		private EventBinding<TestEvent> _textEventBinding;
		private EventBinding<CharacterStatesEvent> _playerEventBinding;

		private void OnDisable()
		{
			EventBus<TestEvent>.DeRegister(_textEventBinding);
			EventBus<CharacterStatesEvent>.DeRegister(_playerEventBinding);
		}

		private void OnEnable()
		{
			_textEventBinding = new EventBinding<TestEvent>(HandleTestEvent);
			EventBus<TestEvent>.Register(_textEventBinding);
			_playerEventBinding = new EventBinding<CharacterStatesEvent>(HandlePlayerEvent);
			EventBus<CharacterStatesEvent>.Register(_playerEventBinding);		
		}

		private void HandleTestEvent (TestEvent testEvent)
		{
			Debug.Log("TestEvent Received");
		}

		private void HandlePlayerEvent(CharacterStatesEvent characterStatesEvent)
		{
			Debug.Log($"Hero HealthCost:{characterStatesEvent.HealthCost},ManaCost:{characterStatesEvent.ManaCost}");
		}

	}
}
