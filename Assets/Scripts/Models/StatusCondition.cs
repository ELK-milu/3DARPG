using Game.EntitySystem;
using System;

namespace Architecture.StateSystem
{

	/// <summary>
	/// 角色的Buff状态抽象基类
	/// </summary>
	public abstract class StatusCondition: IDisposable
	{
		/// <summary>
		/// 状态持续时间
		/// </summary>
		public float ContinueTime;
		private bool Disposed; // 用于跟踪资源是否已经被释放

		public StatusCondition()
		{
		}
		public StatusCondition(float continueTime)
		{
			ContinueTime = continueTime;
		}
		
		public void OnStatus()
		{
			if (ContinueTime > 0)
			{
				Disposed = false;
				Execute();
			}
			else if(!Disposed)
			{
				Dispose();
			}
		}

		/// <summary>
		/// 状态事件让状态自身执行
		/// </summary>
		public abstract void Execute();

		public bool IsOnStatus()
		{
			return Disposed;
		}

		public abstract void Dispose();
	}

	public abstract class CharacterStateCondition : StatusCondition
	{
		protected PlayerController _playerController;
		public CharacterStateCondition (float continueTime, PlayerController playerController)
		{
			base.ContinueTime = continueTime;
			_playerController = playerController;
		}
	}

	public class ExhuastedCondition : CharacterStateCondition
	{
		public ExhuastedCondition(float continueTime, PlayerController playerController) : base(continueTime, playerController) { }
		public override void Execute()
		{
			_playerController.DisableDash = true;
		}

		public override void Dispose()
		{
			_playerController.DisableDash = false;
		}
	}
}
