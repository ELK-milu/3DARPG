using EventPattern.AbilityEvent;
using EventPattern.EventSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Architecture.AbilitySystem
{
	/// <summary>
	/// 技能栏View,所有相关的View处理方法都放在此类，由Controller调用
	/// </summary>
	[DefaultExecutionOrder(-2000)]
	public class AbilityView : MVCView
	{
		[SerializeField]
		public AbilityButton[] Buttons;
		public KeyCode[] KeyCodes =
		{
			KeyCode.Alpha1,KeyCode.Alpha2,KeyCode.Alpha3,
			KeyCode.Alpha4,KeyCode.Alpha5,KeyCode.Alpha6,
			KeyCode.Alpha7,KeyCode.Alpha8,KeyCode.Alpha9
		};

		private AbilityButton _tempAbilityButton;
		private void Awake()
		{
			var AbilitiesPanel = transform.Find("/Canvas/BottomPanel/AbilitiesPanel");
			Buttons = new AbilityButton[AbilitiesPanel.childCount];
			for (int i = 0; i < AbilitiesPanel.childCount; i++)
			{
				_tempAbilityButton = AbilitiesPanel.GetChild(i).GetChild(1).GetComponent<AbilityButton>();
				_tempAbilityButton.Initiate(i,KeyCodes[i]);
				Buttons[i] = _tempAbilityButton;
			}
		}

		/// <summary>
		/// 指定技能更新冷却
		/// </summary>
		/// <param name="progress"></param>
		public void AbilityCoolDown (float progress,int index,AbilityData abilityData)
		{
			if (float.IsNaN(progress))
			{
				progress = 0;
			}
			Buttons[index].isCoolDown = true;
			Buttons[index].CountdownTimer.Reset(abilityData.Duration);
			Buttons[index].CountdownTimer.Start();
			Buttons[index].UpdateCoolDownFill(progress);
		}
		/// <summary>
		/// 为所有技能更新冷却
		/// </summary>
		/// <param name="progress"></param>
		public void CoolDown (float progress)
		{
			if (float.IsNaN(progress))
			{
				progress = 0;
			}
			for (int i = 0; i < Buttons.Length; i++)
			{
				Buttons[i].UpdateRadialFill(progress);
			}
		}
		

		/// <summary>
		/// 更新技能Icon,显示拥有的技能
		/// </summary>
		/// <param name="abilities"></param>
		public void UpdateButtonSprites (IList<Ability> abilities)
		{
			for (int i = 0; i < Buttons.Length; i++)
			{
				if (i < abilities.Count)
				{
					Buttons[i].UpdateButtonSprite(abilities[i].Data.Icon);
				}
				else
				{
					Buttons[i].gameObject.SetActive(false);
				}
			}
		}
	}
	
}
