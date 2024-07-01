using EventPattern.EventSystem;
using EventPattern.PlayerEvent;
using System;
using UnityEngine;

namespace Character.Hero
{
	[RequireComponent(typeof(HeroData))]
	public class HeroController : MonoBehaviour
	{
		public HeroData HeroData;

		private void Awake()
		{
			HeroData = GetComponent<HeroData>();
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.A))
			{
				EventBus<TestEvent>.Raise(new TestEvent());
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				EventBus<PlayerEvent>.Raise(new PlayerEvent
				{
					HealthCost = 100,
					ManaCost = 100
				});
			}
		}

	}

}
