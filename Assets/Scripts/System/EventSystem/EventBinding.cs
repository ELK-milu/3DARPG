using System;

namespace EventPattern.EventSystem
{
	public class EventBinding<T> : IEventBinding<T> where T : IEvent
	{

		#region 重写事件访问属性
		// 不允许外部直接调用
		private event Action<T> OnEvent = @event => { };
		private event Action OnEventNoArgs = () => { };

		Action<T> IEventBinding<T>.OnEvent
		{
			get => OnEvent;
			set => OnEvent = value;
		}
		
		Action IEventBinding<T>.OnEventNoArgs
		{
			get => OnEventNoArgs;
			set => OnEventNoArgs = value;
		}
		#endregion

		public EventBinding (Action<T> onEvent) => this.OnEvent = onEvent;
		public EventBinding (Action onEventNoArgs) => this.OnEventNoArgs = onEventNoArgs;

		public void Add (Action<T> onEvent) => this.OnEvent += onEvent;
		public void Remove(Action<T> onEvent) => this.OnEvent -= onEvent;
		public void Clear(Action<T> onEvent = null) => this.OnEvent = @event => { };
		
		public void Add (Action onEventNoArgs) => this.OnEventNoArgs += onEventNoArgs;
		public void Remove(Action onEventNoArgs) => this.OnEventNoArgs -= onEventNoArgs;
		public void Clear(Action onEventNoArgs = null) => this.OnEventNoArgs = () => { };
	}
}
