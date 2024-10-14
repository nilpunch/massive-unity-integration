using UnityEngine;

namespace Massive.Unity
{
	public class MonoEntitySynchronizer
	{
		private readonly Registry _registry;
		private readonly Pool<MonoEntity> _monoEntityPool = new Pool<MonoEntity>(new MonoEntityFactory());
		private readonly DataSet<MonoEntity> _set = new DataSet<MonoEntity>();
		private Transform _poolRoot;

		public DataSet<MonoEntity> Set => _set;

		public MonoEntitySynchronizer(Registry registry)
		{
			_registry = registry;
		}

		public void SynchronizeAll()
		{
			foreach (var monoEntity in _set.Data.AsSpan(_set.Count))
			{
				var entity = monoEntity.Entity;
				if (!_registry.IsAlive(entity))
				{
					DestroyMonoEntity(entity.Id);
				}
			}

			foreach (var entity in _registry.Entities.Alive)
			{
				if (!_set.IsAssigned(entity))
				{
					CreateMonoEntity(_registry.Entities.GetEntity(entity));
				}
			}
		}

		public void CreateMonoEntity(Entity entity)
		{
			_poolRoot ??= new GameObject("Entity Pool").transform;

			var monoEntity = _monoEntityPool.Get();
			monoEntity.gameObject.SetActive(true);
			monoEntity.transform.SetParent(null);
			monoEntity.Synchronize(_registry, entity);
			_set.Assign(entity.Id, monoEntity);
		}

		public void DestroyMonoEntity(int entityId)
		{
			if (Set.TryGetIndex(entityId, out var packed))
			{
				var monoEntity = Set.Data[packed];
				monoEntity.gameObject.SetActive(false);
				monoEntity.transform.SetParent(_poolRoot);
				_monoEntityPool.Return(monoEntity);
				_set.Unassign(entityId);
			}
		}
	}
}
