using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EventPattern.EventSystem
{
	public static class EventBusUtil
	{
		public static IReadOnlyList<Type> EventTypes { get; set; }
		public static IReadOnlyList<Type> EventBusTypes { get; set; }

		/// <summary>
		/// 在场景加载前利用反射读取程序集中的所有IEvent类型，并加载所有类型的事件总线
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void Initialize()
		{
			EventTypes = PredefinedAssemblyUtil.GetTypes(typeof(IEvent));
			EventBusTypes = InitializeAllBuses();
		}
#if UNITY_EDITOR
		public static PlayModeStateChange PlayModeState { get; set; }
    
		/// <summary>
		/// Initializes the Unity Editor related components of the EventBusUtil.
		/// The [InitializeOnLoadMethod] attribute causes this method to be called every time a script
		/// is loaded or when the game enters Play Mode in the Editor. This is useful to initialize
		/// fields or states of the class that are necessary during the editing state that also apply
		/// when the game enters Play Mode.
		/// The method sets up a subscriber to the playModeStateChanged event to allow
		/// actions to be performed when the Editor's play mode changes.
		/// </summary>    
		[InitializeOnLoadMethod]
		public static void InitializeEditor() {
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}
    
		static void OnPlayModeStateChanged(PlayModeStateChange state) {
			PlayModeState = state;
			if (state == PlayModeStateChange.ExitingPlayMode) {
				ClearAllBuses();
			}
		}
#endif

		static List<Type> InitializeAllBuses()
		{
			List<Type> eventBusTypes = new List<Type>();
			var typedef = typeof(EventBus<>);
			foreach (var eventType in EventTypes)
			{
				var busType = typedef.MakeGenericType(eventType);
				eventBusTypes.Add(busType);
				Debug.LogFormat("<color=#00ff00>{0}</color>", $"初始化{eventType.Name}总线");
			}
			return eventBusTypes;
		}
		
		/// <summary>
		/// Clears (removes all listeners from) all event buses in the application.
		/// </summary>
		public static void ClearAllBuses() {
			Debug.LogFormat("<color=#ff0000>{0}</color>", "清除所有事件总线...");
			for (int i = 0; i < EventBusTypes.Count; i++) {
				var busType = EventBusTypes[i];
				var clearMethod = busType.GetMethod("Clear", BindingFlags.Static | BindingFlags.NonPublic);
				clearMethod?.Invoke(null, null);
			}
		}
	}
}
