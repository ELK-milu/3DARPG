using UnityEngine;

namespace Game.EntitySystem
{
	public interface ISpawnPointStrategy
	{
		Transform NextSpawnPoint();
	}

}
