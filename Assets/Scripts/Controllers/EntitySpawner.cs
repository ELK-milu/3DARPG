namespace Game.EntitySystem
{
	/// <summary>
	/// 让EntitySpawner持有接口而非继承接口，则生成器可通用，不必定义新的生成器类，只需要更换类内部的策略即可
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EntitySpawner<T> where T : Entity
	{
		private IEntityFactory<T> _entityFactory;
		private ISpawnPointStrategy _spawnPointStrategy;

		public EntitySpawner (IEntityFactory<T> entityFactory, ISpawnPointStrategy spawnPointStrategy)
		{
			_entityFactory = entityFactory;
			_spawnPointStrategy = spawnPointStrategy;
		}

		public T Spawn()
		{
			return _entityFactory.Create(_spawnPointStrategy.NextSpawnPoint());
		}
	}

}
