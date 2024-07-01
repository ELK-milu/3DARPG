namespace Architecture.AbilitySystem
{
	/// <summary>
	/// 通过泛型限定，保证按照格式继承该类
	/// </summary>
	/// <typeparam name="Tdata">包含基本数值的数据类</typeparam>
	/// <typeparam name="TdataContainer">数据容器类，用于执行各类方法</typeparam>
	/// <typeparam name="Tmodel"></typeparam>
	/// <typeparam name="Tview"></typeparam>
	public abstract class MVCController<Tdata,TdataContainer,Tmodel,Tview> where Tdata:ModelData 
	where TdataContainer: DataContainer<Tdata>,new()
	where Tmodel : MVCModel<Tdata,TdataContainer>,new()
	{
		protected Tmodel _model { get; set;}
		protected Tview _view { get; set;}

		protected abstract void ConnectModel();
		protected abstract void ConnectView();

		#region MyRegion
		protected MVCController()
		{
			
		}
		protected MVCController (Tview view, Tmodel model)
		{
			_view = view;
			_model = model;
			ConnectModel();
			ConnectView();
		}
		protected MVCController(Tdata[] datas,Tview view)
		{
			var model = new Tmodel();
			foreach (var abilityData in datas)
			{
				var ability = new TdataContainer();
				ability.GetData(abilityData);
				model.Add(ability);
			}
			Preconditions.CheckNotNull(view);
			_view = view;
			_model = model;
			ConnectModel();
			ConnectView();
		}
		#endregion 需要继承的构造函数
	};

}
