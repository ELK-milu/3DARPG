using Architecture;
using UnityEngine;

namespace Game.EntitySystem
{
	[CreateAssetMenu(menuName = "ScriptableObjects/EntityData", fileName = "EntityData", order = 0)]
	public abstract class EntityData : ModelData
	{
		// 拾取时获得的金币
		public int GetGold;
		public GameObject Prefab;
	}

}
