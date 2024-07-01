using System.Collections.Generic;
using UnityEngine;

namespace EventPattern.EventSystem
{
	public static class EventBus<T> where T : IEvent
	{
		/// <summary>
		/// 存储事件的哈希表
		/// </summary>
		private static readonly HashSet<IEventBinding<T>> _bindings = new HashSet<IEventBinding<T>>();

		public static void Register (EventBinding<T> binding) => _bindings.Add(binding);
		public static void DeRegister (EventBinding<T> binding) => _bindings.Remove(binding);

		public static void Raise (T @event)
		{
			foreach (var binding in _bindings)
			{
				binding.OnEvent.Invoke(@event);
				binding.OnEventNoArgs.Invoke();
			}
		}

		static void Clear()
		{
			_bindings.Clear();
		}

	}

}
