using System;
using System.Collections;
using System.Collections.Generic;

namespace ObserverPattern.Observable
{
	[Serializable]
	public class ObservableList<T> : IList<T>, IObservableList<T>
	{
		// 接受的泛型列表
		private readonly IList<T> _list;
		

		/// <summary>
		/// 任意事件触发的委托(event修饰后不允许外部触发委托)
		/// </summary>
		public event Action<IList<T>> AnyValueChanged;

		public ObservableList (IList<T> list = null)
		{
			_list = list ?? new List<T>();
		}

		// 定义索引函数,可访问数组
		public T this [int index]
		{
			get => _list[index];
			set
			{
				_list[index] = value;
				Invoke();
			}
		}

		public void Invoke()
		{
			AnyValueChanged?.Invoke(_list);
		}

		#region 根据_list重写IList中的变量
		public int Count => _list.Count;
		public bool IsReadOnly => _list.IsReadOnly;
		#endregion


		#region 根据_list重写IList中的方法
		public void Add (T item)
		{
			_list.Add(item);
			Invoke();
		}

		public void Clear()
		{
			_list.Clear();
			Invoke();
		}

		public bool Contains (T item) => _list.Contains(item);
		public void CopyTo (T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

		public bool Remove (T item)
		{
			var result = _list.Remove(item);
			if (result)
			{
				Invoke();
			}
			return result;
		}

		// 重写迭代器方法
		public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

		public int IndexOf (T item) => _list.IndexOf(item);
		public void Insert (int index, T item) => _list.Insert(index, item);
		public void RemoveAt (int index) => _list.RemoveAt(index);
		#endregion

	}
}
