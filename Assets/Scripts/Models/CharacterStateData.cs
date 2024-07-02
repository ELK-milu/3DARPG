using EventPattern.PlayerEvent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Architecture.StateSystem
{
	/// <summary>
	/// 角色状态属性类
	/// </summary>
	[CreateAssetMenu(fileName = "CharacterStateData",menuName = "ScriptableObjects/CharacterStateData")]
	public class CharacterStateData : ModelData
	{
		[Tooltip("角色的Buff状态列表")]
		public List<StatusCondition> StatusConditions;
		[Tooltip("角色的属性")]
		public Attribution CharacterAttribution;
		// 触发的事件
		[FormerlySerializedAs("PlayerEvent")]
		public CharacterStatesEvent characterStatesEvent = new CharacterStatesEvent();

	}
}
