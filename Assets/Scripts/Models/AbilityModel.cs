using CommandPattern.Commands;
using UnityEngine;

namespace Architecture.AbilitySystem
{
	/// <summary>
	/// 管理所有能力的模型类
	/// </summary>
	[DefaultExecutionOrder(-1999)]
	public class AbilityModel : MvcModel<AbilityData,Ability>
	{

	}

	/// <summary>
	/// 技能类，包含了技能的属性
	/// </summary>
	public class Ability : DataContainer<AbilityData>
	{
		public Ability() : base() { }
		public Ability (AbilityData data):base(data){}
		public new AbilityCommand CreateCommand()
		{
			return new AbilityCommand(Data);
		}
	}

}
