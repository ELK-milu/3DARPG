using EventPattern.AbilityEvent;
using EventPattern.PlayerEvent;
using UnityEngine;

namespace Architecture.AbilitySystem
{
	/// <summary>
	/// 技能属性类
	/// </summary>
	[CreateAssetMenu(fileName = "AbilityData",menuName = "ScriptableObjects/AbilityData")]
	public class AbilityData : ModelData
	{
		[Tooltip("绑定的技能动画")]
		public AnimationClip AnimationClip;
		[Tooltip("存储动画的哈希值")]
		public int AnimationHash;
		public float Duration;
		public Sprite Icon;
		[Tooltip("存储技能消耗")]
		public Vector2Int AbilityCost;

		private CharacterStatesEvent _characterStatesEvent = new CharacterStatesEvent();
		/// <summary>
		/// 技能释放存储的玩家事件
		/// </summary>
		/// <returns></returns>
		public CharacterStatesEvent CharacterStatesEvent {
			get
			{
				return _characterStatesEvent;
			}
		}

		private AbilityEvent _abilityEvent = new AbilityEvent(0);

		public AbilityEvent AbilityEvent 
		{
			get
			{
				return _abilityEvent;
			}
		}

		// ScriptableObject在初始化或值被修改时会调用OnValidate
		private void OnValidate()
		{
			//AnimationHash = Animator.StringToHash(this.AnimationClip.name);
			SetPlayerEvent(AbilityCost.x,AbilityCost.y);
			SetAbilityEvent();
		}

		public void SetPlayerEvent (int healthCost, int manaCost)
		{
			_characterStatesEvent.HealthCost = healthCost;
			_characterStatesEvent.ManaCost = manaCost;
		}

		public void SetAbilityEvent()
		{
			_abilityEvent.Duration = Duration;
		}
		
	}

}
