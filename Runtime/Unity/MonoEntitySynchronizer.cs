using UnityEngine;

namespace Massive.Unity
{
	public class MonoEntitySynchronizer
	{
		private readonly IRegistry _registry;
		private readonly Pool<MonoEntity> _monoEntityPool = new Pool<MonoEntity>(new MonoEntityFactory());
		private readonly DataSet<MonoEntity> _set = new DataSet<MonoEntity>();
		private Transform _poolRoot;

		public IReadOnlyDataSet<MonoEntity> Set => _set;

		public MonoEntitySynchronizer(IRegistry registry)
		{
			_registry = registry;
		}

		public void SynchronizeAll()
		{
			foreach (var monoEntity in _set.Data)
			{
				var entity = monoEntity.Entity;
				if (!_registry.IsAlive(entity))
				{
					DestroyMonoEntity(entity.Id);
				}
			}

			foreach (var entity in _registry.Entities.Alive)
			{
				if (!_set.IsAssigned(entity.Id))
				{
					CreateMonoEntity(entity);
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
			if (Set.TryGetDense(entityId, out var dense))
			{
				var monoEntity = Set.Data[dense];
				monoEntity.gameObject.SetActive(false);
				monoEntity.transform.SetParent(_poolRoot);
				_monoEntityPool.Return(monoEntity);
				_set.Unassign(entityId);
			}
		}
	}
}
