
using UnityEngine;
using Utilities;

namespace Game.EntitySystem
{
	public abstract class EntitySpawnManager:MonoBehaviour
	{
		[SerializeField] protected SpawnPointStrategyType _spawnPointStrategyType = SpawnPointStrategyType.Linear;
		[SerializeField] protected Transform[] _spawnPoints;
		[SerializeField] protected float _spawnInterval = 1f;
		protected CountdownTimer _spawnTimer;
		protected ISpawnPointStrategy _spawnPointStrategy;

		protected enum SpawnPointStrategyType
		{
			Linear,
			Random
		}
		
		public virtual void Awake()
		{
			_spawnPointStrategy = _spawnPointStrategyType switch
			{
				SpawnPointStrategyType.Linear => new LinearSpawnPointStrategy(_spawnPoints),
				SpawnPointStrategyType.Random => new RandomSpawnPointStrategy(_spawnPoints),
				_ => _spawnPointStrategy
			};
		}

		public abstract void Spawn();
	}

}
