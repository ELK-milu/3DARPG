using EventPattern.EventSystem;
using EventPattern.PlayerEvent;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Character.Hero
{
	public class HeroData:MonoBehaviour
	{
		private EventBinding<TestEvent> _textEventBinding;
		private EventBinding<PlayerEvent> _playerEventBinding;

		private void OnDisable()
		{
			EventBus<TestEvent>.DeRegister(_textEventBinding);
			EventBus<PlayerEvent>.DeRegister(_playerEventBinding);
		}

		private void OnEnable()
		{
			_textEventBinding = new EventBinding<TestEvent>(HandleTestEvent);
			EventBus<TestEvent>.Register(_textEventBinding);
			_playerEventBinding = new EventBinding<PlayerEvent>(HandlePlayerEvent);
			EventBus<PlayerEvent>.Register(_playerEventBinding);		
		}

		private void HandleTestEvent (TestEvent testEvent)
		{
			Debug.Log("TestEvent Received");
		}

		private void HandlePlayerEvent(PlayerEvent playerEvent)
		{
			Debug.Log($"Hero HealthCost:{playerEvent.HealthCost},ManaCost:{playerEvent.ManaCost}");
		}

	}
}
