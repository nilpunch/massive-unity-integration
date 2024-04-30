using UnityEngine;

namespace Massive.Unity
{
	public class MonoEntitiesDataBase
	{
		private readonly Pool<MonoEntity> _monoEntityPool = new Pool<MonoEntity>(new MonoEntityFactory());

		public DataSet<MonoEntity> MonoEntities { get; } = new DataSet<MonoEntity>();

		public void CreateMonoEntity(Entity entity)
		{
			var monoEntity = _monoEntityPool.Get();
			monoEntity.gameObject.SetActive(true);
			monoEntity.Synchronize(entity);
			MonoEntities.Assign(entity.Id, monoEntity);
		}

		public void DestroyMonoEntity(int entityId)
		{
			if (MonoEntities.TryGetDense(entityId, out var dense))
			{
				MonoEntities.Unassign(entityId);
				var monoEntity = MonoEntities.Data[dense];
				monoEntity.gameObject.SetActive(false);
				_monoEntityPool.Return(monoEntity);
			}
		}
	}
}