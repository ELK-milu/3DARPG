using System;

namespace StatePattern.StateSystem
{
	public class FuncPredicate : IPredicate
	{

		readonly Func<bool> _func;

		public FuncPredicate (Func<bool> func)
		{
			_func = func;
		}
		
		/// <summary>
		/// 用委托返回状态切换标识
		/// </summary>
		/// <returns></returns>
		public bool Evaluate()=> _func.Invoke();
	}
}
