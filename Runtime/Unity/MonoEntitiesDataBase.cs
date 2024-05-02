using UnityEngine;

namespace Massive.Unity
{
	public class MonoEntitiesDataBase
	{
		private readonly IRegistry _registry;
		private readonly Transform _poolRoot = new GameObject("Entity Pool").transform;
		private readonly Pool<MonoEntity> _monoEntityPool = new Pool<MonoEntity>(new MonoEntityFactory());
		private readonly DataSet<MonoEntity> _set = new DataSet<MonoEntity>();

		public IReadOnlyDataSet<MonoEntity> Set => _set;

		public MonoEntitiesDataBase(IRegistry registry)
		{
			_registry = registry;
		}
		
		public void CreateMonoEntity(Entity entity)
		{
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