using Architecture.StateSystem;
using CommandPattern.CommandSystem;
using EventPattern.EventSystem;
using EventPattern.PlayerEvent;

namespace CommandPattern.Commands
{
	/// <summary>
	/// 角色状态发送指令
	/// </summary>
	public class CharacterStateCommand : ICommand
	{
		private readonly CharacterStateData _data;
		public CharacterStateCommand (CharacterStateData data)
		{
			_data = data;
		}

		public void Execute()
		{
			EventBus<CharacterStatesEvent>.Raise(_data.characterStatesEvent);
		}
	}
}
