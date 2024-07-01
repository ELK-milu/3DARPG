using EventPattern.AbilityEvent;
using EventPattern.EventSystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace Architecture.AbilitySystem
{
	/// <summary>
	/// 技能按钮View
	/// </summary>
	[DefaultExecutionOrder(-2001)]
	public class AbilityButton : MonoBehaviour
	{
		public Image CoolDownImage { get; private set; }

		public Image RadialImage { get; private set; }
		public Image AbilityIcon { get; private set; }
		public TextMeshProUGUI KeyText { get; private set; }
		public Button SelfBtn { get; private set; }
		public int Index { get; set; }
		public KeyCode Key { get; private set; }
		public bool isCoolDown;
		public CountdownTimer CountdownTimer = new CountdownTimer(0);

		/// <summary>
		/// int代表了当前技能在技能栏中的序号，并传给委托中的方法(用于和对应序号的Ability类进行交互)
		/// </summary>
		public event Action<int> OnButtonPressed = (@index)=>{  };

		public EventBinding<AbilityEvent> AbilityEventBinding;
		
		public void Initiate (int index, KeyCode key)
		{
			ComponentInitiate();
			isCoolDown = false;
			Index = index;
			Key = key;
			KeyText.text = Key.ToString();
		}

		private void ComponentInitiate()
		{
			if (!CoolDownImage)
			{
				CoolDownImage = transform.Find("CoolDownImage").GetComponent<Image>();
			}
			if (!RadialImage)
			{
				RadialImage = transform.Find("RadialImage").GetComponent<Image>();
			}
			if (!AbilityIcon)
			{
				AbilityIcon = transform.Find("AbilityIcon").GetComponent<Image>();
			}
			if (!SelfBtn)
			{
				SelfBtn = transform.Find("AbilityIcon").GetComponent<Button>();
			}
			if (!KeyText)
			{
				KeyText = CoolDownImage.transform.Find("IndexTMP").GetComponent<TextMeshProUGUI>();
			}
			if (Index == 0)
			{
				Index = transform.GetSiblingIndex();
			}
		}
		private void Awake()
		{
			ComponentInitiate();
			SelfBtn.onClick.AddListener(() => OnButtonPressed(Index));
		}

		private void Update()
		{
			if (Input.GetKeyDown(Key))
			{
				OnButtonPressed.Invoke(Index);
			}
			if (isCoolDown)
			{
				CountdownTimer.Tick(Time.deltaTime);
				UpdateCoolDownFill(CountdownTimer.Progress);
			}
			if (CountdownTimer.Progress <= 0)
			{
				isCoolDown = false;
			}
		}

		public void RegisterListener (Action<int> listener)
		{
			OnButtonPressed += listener;
		}

		public void UpdateButtonSprite (Sprite newIcon)
		{
			if (!AbilityIcon)
			{
				ComponentInitiate();
			}
			AbilityIcon.sprite = newIcon;
		}

		public void UpdateRadialFill (float progress)
		{
			if (RadialImage)
			{
				RadialImage.fillAmount = progress;
			}
			else
			{
				ComponentInitiate();
			}
		}
		public void UpdateCoolDownFill (float progress)
		{
			if (CoolDownImage)
			{
				CoolDownImage.fillAmount = progress;
			}
			else
			{
				ComponentInitiate();
			}
		}

	}
}