using UnityEngine;
using Utilities;

namespace Game.EntitySystem
{
	public class CollectibleSpawnManager : EntitySpawnManager
	{
		[SerializeField] private CollectibleData[] _collectibleDatas;
		protected EntitySpawner<Collectable> _entitySpawner;
		private int _count = 0;
		
		public override void Awake()
		{
			base.Awake();
			_entitySpawner = new EntitySpawner<Collectable>(new EntityFactory<Collectable>(_collectibleDatas), _spawnPointStrategy);
			_spawnTimer = new CountdownTimer(_spawnInterval);
			_spawnTimer.OnTimerStop += () =>
			{
				if (_count >= _spawnPoints.Length)
				{
					_spawnTimer.Stop();
					return;
				}
				Spawn();
				_count++;
				_spawnTimer.Start();
			};
		}

		public override void Spawn()
		{
			_entitySpawner.Spawn();
		}
	}
}
