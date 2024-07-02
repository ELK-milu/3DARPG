using EventPattern.EventSystem;
using EventPattern.PlayerEvent;
using UnityEngine;

public class LiquidValueState : MonoBehaviour
{
	public Liquid LiquidBall;
	public GameObject BallObj;
	
	public EventBinding<CharacterStatesEvent> CharacterStatesEventBinding;

	private void Awake()
	{
		CharacterStatesEventBinding = new EventBinding<CharacterStatesEvent>(() => { });
	}

}