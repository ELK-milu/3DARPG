using System.Collections.Generic;
using UnityEngine;

namespace Game.EntitySystem
{
	public class RandomSpawnPointStrategy : ISpawnPointStrategy
	{
		private List<Transform> _unUsedSpawnPoints;
		private Transform[] _spawnPoints;

		public RandomSpawnPointStrategy (Transform[] spawnPoints)
		{
			_spawnPoints = spawnPoints;
			_unUsedSpawnPoints = new List<Transform>(spawnPoints);
		}
		/// <summary>
		/// 获取随机生成点，每获取一个位置则将其从队列中移除，若队列为空则重新生成原队列再获取随机生成点
		/// </summary>
		/// <returns></returns>
		public Transform NextSpawnPoint()
		{
			if (_unUsedSpawnPoints.Count == 0)
			{
				_unUsedSpawnPoints = new List<Transform>(_spawnPoints);
			}
			var index = Random.Range(0, _unUsedSpawnPoints.Count);
			var nextSpawnPoint = _unUsedSpawnPoints[index];
			_unUsedSpawnPoints.RemoveAt(index);
			return nextSpawnPoint;
		}
	}

}
