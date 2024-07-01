using Architecture.AbilitySystem;
using CommandPattern.CommandSystem;
using EventPattern.AbilityEvent;
using EventPattern.EventSystem;
using EventPattern.PlayerEvent;
using Unity.VisualScripting;
using UnityEngine;

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
			EventBus<PlayerEvent>.Raise(_data.PlayerEvent);
			EventBus<AbilityEvent>.Raise(_data.AbilityEvent);
		}
	}

}
