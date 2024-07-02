using CommandPattern.Commands;
using EventPattern.AbilityEvent;
using EventPattern.EventSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utilities;

namespace Architecture.AbilitySystem
{
	/// <summary>
	/// AbilityController包含AbilityModel和AbilityView
	/// AbilityModel管理技能类Ability，Ability包含技能数据类AbilityData，触发时发送AbilityCommand
	/// AbilityView包含AbilityButton等UI脚本
	/// </summary>
	[DefaultExecutionOrder(-1900)]
	public class AbilityController : MvcController<AbilityData,Ability,AbilityModel,AbilityView>
	{
		/// <summary>
		/// 技能触发的指令队列
		/// </summary>
		private readonly Queue<AbilityCommand> _abilityCommandQueue = new Queue<AbilityCommand>();
		
		/// <summary>
		/// 技能输入的冷却时间，非单个技能的冷却
		/// </summary>
		private readonly CountdownTimer _countdownTimer = new CountdownTimer(1);

		#region 继承父构造函数
		public AbilityController (AbilityView view, AbilityModel model) : base(view, model) { }
		public AbilityController (AbilityData[] datas, AbilityView view) : base(datas,view) { }
		
		override protected void ConnectModel()
		{
			UpdateButton(_model.DataContainers);
			_model.DataContainers.AnyValueChanged += UpdateButton;
		}
		override protected void ConnectView()
		{
			RegisterButtonListener();
		}
		#endregion

		#region 生命周期

		public void Update (float deltaTime)
		{
			// 更新技能输入冷却
			_countdownTimer.Tick(deltaTime);
			_view.CoolDown(_countdownTimer.Progress);

			if (!_countdownTimer.IsRunning && _abilityCommandQueue.TryDequeue(out AbilityCommand cmd))
			{
				cmd.Execute();
				_countdownTimer.Reset();
				_countdownTimer.Start();
			}
		}

		/// <summary>
		/// 为view中的Button添加事件监听
		/// </summary>
		public void OnEnable()
		{
			for (int i = 0; i < _view.Buttons.Length; i++)
			{
				_view.Buttons[i].AbilityEventBinding = new EventBinding<AbilityEvent>(() => { });
				EventBus<AbilityEvent>.Register(_view.Buttons[i].AbilityEventBinding);
			}
		}

		public void OnDisable()
		{
			for (int i = 0; i < _view.Buttons.Length; i++)
			{
				EventBus<AbilityEvent>.DeRegister(_view.Buttons[i].AbilityEventBinding);
			}
		}
		
		
		#endregion
		
		
		#region 作为中间处理，由Model中委托调用后触发View中的方法
		
		/// <summary>
		/// 更新技能列表后刷新所有的Button图标
		/// </summary>
		/// <param name="abilities"></param>
		private void UpdateButton (IList<Ability> abilities)
		{
			_view.UpdateButtonSprites(abilities);
		}

		/// <summary>
		/// 为View中的Button事件委托调用Model中Ability的发送命令方法
		/// </summary>
		private void RegisterButtonListener()
		{
			for (int i = 0; i < _view.Buttons.Length; i++)
			{
				_view.Buttons[i].RegisterListener(OnAbilityButtonPressed);
			}
		}

		/// <summary>
		/// 触发技能，冷却时间时不触发
		/// </summary>
		/// <param name="index"></param>
		private void OnAbilityButtonPressed(int index)
		{
			if ((_countdownTimer.Progress < 0.25f || !_countdownTimer.IsRunning) && !_view.Buttons[index].isCoolDown)
			{
				if (_model.DataContainers[index] != null)
				{
					_view.AbilityCoolDown(1, index,_model.DataContainers[index].Data);
					_abilityCommandQueue.Enqueue(_model.DataContainers[index].CreateCommand());
				}
			}
			EventSystem.current.SetSelectedGameObject(null);
		}
		#endregion

	}

}
