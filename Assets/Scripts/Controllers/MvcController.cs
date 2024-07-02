namespace Architecture
{
	/// <summary>
	/// 通过泛型限定，保证按照格式继承该类
	/// </summary>
	/// <typeparam name="TData">
    /// <para>包含基本数值的数据类</para><code>()=&gt; myProperty</code></typeparam>
	/// <typeparam name="TDataContainer">
    /// <para>数据容器类，用于执行各类方法</para><code>()=&gt; myProperty</code></typeparam>
	/// <typeparam name="TMvcModel"></typeparam>
	/// <typeparam name="TMvcView"></typeparam>
	public abstract class MvcController<TData,TDataContainer,TMvcModel,TMvcView> where TData:ModelData 
	where TDataContainer: DataContainer<TData>,new()
	where TMvcModel : MvcModel<TData,TDataContainer>,new()
	where TMvcView : MvcView,new()
	{
		protected TMvcModel _model { get; set;}
		protected TMvcView _view { get; set;}

		protected abstract void ConnectModel();
		protected abstract void ConnectView();

		#region MyRegion
		protected MvcController()
		{
			
		}
		protected MvcController (TMvcView view, TMvcModel model)
		{
			_view = view;
			_model = model;
			ConnectModel();
			ConnectView();
		}
		protected MvcController(TData[] datas,TMvcView view)
		{
			var model = new TMvcModel();
			foreach (var abilityData in datas)
			{
				var ability = new TDataContainer();
				ability.GetData(abilityData);
				model.Add(ability);
			}
			Preconditions.CheckNotNull(view);
			_view = view;
			_model = model;
			ConnectModel();
			ConnectView();
		}
		#endregion
	};

}
