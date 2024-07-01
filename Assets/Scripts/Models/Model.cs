using ObserverPattern.Observable;
using System;

namespace Architecture.AbilitySystem
{
	
	public class MVCModel<T1,T2> where T1 : ModelData where T2 : DataContainer<T1>
	{
		public readonly ObservableList<T2> DataContainers = new();
		public Type GetDataType()
		{
			return typeof(T2);
		}
		public void Add (T2 ability)
		{
			DataContainers.Add(ability);
		}
	}
}
