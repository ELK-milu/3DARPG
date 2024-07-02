using EventPattern.EventSystem;
using EventPattern.PlayerEvent;
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
				EventBus<CharacterStatesEvent>.Raise(new CharacterStatesEvent
				{
					HealthCost = 100,
					ManaCost = 100
				});
			}
		}

	}

}
