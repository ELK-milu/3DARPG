using Architecture.AbilitySystem;
using EventPattern.EventSystem;
using EventPattern.PlayerEvent;

namespace Architecture.StateSystem
{
	public class CharacterStateController : MvcController<CharacterStateData,CharacterState,CharacterStateModel,CharacterStateView>
	{
		public CharacterStateController (CharacterStateView view, CharacterStateModel model) : base(view, model) { }
		public CharacterStateController (CharacterStateData[] datas, CharacterStateView view) : base(datas,view) { }

		override protected void ConnectModel()
		{
			
		}

		override protected void ConnectView()
		{
		}

		public void OnEnable()
		{
			foreach (var liquidValueState in _view.LiquidValueStates)
			{
				EventBus<CharacterStatesEvent>.Register(liquidValueState.CharacterStatesEventBinding);
			}
		}

		public void OnDisable()
		{
			foreach (var liquidValueState in _view.LiquidValueStates)
			{
				EventBus<CharacterStatesEvent>.DeRegister(liquidValueState.CharacterStatesEventBinding);
			}
		}

	}
}
