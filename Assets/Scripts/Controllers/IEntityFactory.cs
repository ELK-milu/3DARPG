using UnityEngine;

namespace Game.EntitySystem
{
	// 本来想把对象池类放置于工厂类内部管理实体
	// 但是考虑到单一责任的原则，对象池类还是另外定义吧
	
	/// <summary>
	/// 生成实体的工厂类
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IEntityFactory<T> where T : Entity
	{
		T Create (Transform transform);
	}

	public class EntityFactory<T> : IEntityFactory<T> where T : Entity
	{
		// 存放实体数据的数组
		private EntityData[] _datas;
		public EntityFactory (EntityData[] datas)
		{
			_datas = datas;
		}
		public T Create (Transform spawnPoint)
		{
			var entityData = _datas[Random.Range(0, _datas.Length)];
			return GameObject.Instantiate(entityData.Prefab, spawnPoint.position,spawnPoint.rotation).GetComponent<T>();
		}
	}

}
