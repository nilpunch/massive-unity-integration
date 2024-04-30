using UnityEngine;

namespace Massive.Unity
{
	public class MonoEntitiesDataBase
	{
		private readonly IRegistry _registry;
		private readonly Pool<MonoEntity> _monoEntityPool = new Pool<MonoEntity>(new MonoEntityFactory());

		public DataSet<MonoEntity> MonoEntities { get; } = new DataSet<MonoEntity>();

		public MonoEntitiesDataBase(IRegistry registry)
		{
			_registry = registry;
		}
		
		public void CreateMonoEntity(Entity entity)
		{
			var monoEntity = _monoEntityPool.Get();
			monoEntity.gameObject.SetActive(true);
			monoEntity.Synchronize(_registry, entity);
			MonoEntities.Assign(entity.Id, monoEntity);
		}

		public void DestroyMonoEntity(int entityId)
		{
			if (MonoEntities.TryGetDense(entityId, out var dense))
			{
				var monoEntity = MonoEntities.Data[dense];
				monoEntity.gameObject.SetActive(false);
				_monoEntityPool.Return(monoEntity);
				MonoEntities.Unassign(entityId);
			}
		}
	}
}