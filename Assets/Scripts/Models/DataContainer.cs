namespace Architecture.AbilitySystem
{
	public class DataContainer<T> where T : ModelData
	{
		// 需要被new覆盖
		public T Data;
		// 继承时必须实现无参和有参构造方法
		protected DataContainer()
		{
			
		}
		protected DataContainer (T data)
		{
			this.Data = data;
		}
		public void GetData (T data)
		{
			this.Data = data;
		}
		public void CreateCommand()
		{
			
		}
	}
}
