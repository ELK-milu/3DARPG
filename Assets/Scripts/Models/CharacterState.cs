using CommandPattern.Commands;

namespace Architecture.StateSystem
{
	public class CharacterState : DataContainer<CharacterStateData>
	{
		public CharacterState() : base() { }
		public CharacterState (CharacterStateData data):base(data){}
		public new CharacterStateCommand CreateCommand()
		{
			return new CharacterStateCommand(Data);
		}
	}
}
