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
		public HashSet<StatusCondition> StatusConditions;
		[Tooltip("角色的属性")]
		[SerializeField]
		public Attribution CharacterAttribution;
		// 触发的事件
		[FormerlySerializedAs("PlayerEvent")]
		public CharacterStatesEvent characterStatesEvent = new CharacterStatesEvent();

		private void Awake()
		{
			ResetAttribution();
		}

		public void ResetAttribution()
		{
			StatusConditions = new HashSet<StatusCondition>();
			CharacterAttribution.CurrentHealth = CharacterAttribution.MaxHealth;
			CharacterAttribution.CurrentMana = CharacterAttribution.MaxMana;
			CharacterAttribution.CurrentStamina = CharacterAttribution.MaxStamina;
		}

		public void Update()
		{
			if(StatusConditions == null) return;
			foreach (var status in StatusConditions)
			{
				status?.Execute();	
			}
		}

	}
}
