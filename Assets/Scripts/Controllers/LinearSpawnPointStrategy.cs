using UnityEngine;

namespace Game.EntitySystem
{
	/// <summary>
	/// 按列表顺序生成
	/// </summary>
	public class LinearSpawnPointStrategy : ISpawnPointStrategy
	{
		private Transform[] _spawnPoints;
		private int _currentIndex;

		public LinearSpawnPointStrategy (Transform[] spawnPoints)
		{
			_spawnPoints = spawnPoints;
			_currentIndex = 0;
		}
		
		public Transform NextSpawnPoint()
		{
			var nextSpawnPoint = _spawnPoints[_currentIndex];
			_currentIndex = (_currentIndex + 1) % _spawnPoints.Length;
			return nextSpawnPoint;
		}
	}

}
