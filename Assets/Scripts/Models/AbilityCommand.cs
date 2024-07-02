using Architecture.AbilitySystem;
using CommandPattern.CommandSystem;
using EventPattern.AbilityEvent;
using EventPattern.EventSystem;
using EventPattern.PlayerEvent;

namespace CommandPattern.Commands
{
	/// <summary>
	/// 能力指令,接受AbilityData作为入参
	/// </summary>
	public class AbilityCommand:ICommand
	{
		private readonly AbilityData _data;
		public float Duration => _data.Duration;

		public AbilityCommand (AbilityData data)
		{
			_data = data;
		}

		public void Execute()
		{
			EventBus<CharacterStatesEvent>.Raise(_data.CharacterStatesEvent);
			EventBus<AbilityEvent>.Raise(_data.AbilityEvent);
		}
	}

}
