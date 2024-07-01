using System.Collections.Generic;
using UnityEngine;



namespace Architecture.StateSystem
{
	/// <summary>
	/// 角色状态属性类
	/// </summary>
	[CreateAssetMenu(fileName = "StateData",menuName = "ScriptableObjects/StateData")]
	public class StateData : ModelData
	{
		[Tooltip("角色的Buff状态列表")]
		public List<StatusCondition> StatusConditions;
		[Tooltip("角色的属性")]
		public Attribution CharacterAttribution;
	}
}
