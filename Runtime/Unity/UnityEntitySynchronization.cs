using System;
using Object = UnityEngine.Object;

namespace Massive.Unity
{
	public class UnityEntitySynchronization : IDisposable
	{
		private readonly Registry _registry;
		private readonly EntityViewSynchronizer _entityViewSynchronizer;
		private readonly MonoEntitySynchronizer _monoEntities;

		public UnityEntitySynchronization(Registry registry, EntityViewPool entityViewPool)
		{
			_registry = registry;
			_entityViewSynchronizer = new EntityViewSynchronizer(registry, entityViewPool);
			_monoEntities = new MonoEntitySynchronizer(registry);
		}

		public void SubscribeEntities()
		{
			_registry.Entities.AfterCreated += OnAfterEntityCreated;
			_registry.Entities.BeforeDestroyed += OnBeforeEntityDestroyed;
		}

		public void SubscribeViews()
		{
			_registry.Set<ViewAsset>().AfterAssigned += OnAfterViewAssigned;
			_registry.Set<ViewAsset>().BeforeUnassigned += OnBeforeViewUnassigned;
		}
		
		public void UnsubscribeEntities()
		{
			_registry.Entities.AfterCreated -= OnAfterEntityCreated;
			_registry.Entities.BeforeDestroyed -= OnBeforeEntityDestroyed;
		}

		public void UnsubscribeViews()
		{
			_registry.Set<ViewAsset>().AfterAssigned -= OnAfterViewAssigned;
			_registry.Set<ViewAsset>().BeforeUnassigned -= OnBeforeViewUnassigned;
		}
		
		public void SynchronizeEntities()
		{
			_monoEntities.SynchronizeAll();
		}

		public void SynchronizeViews()
		{
			_entityViewSynchronizer.SynchronizeAll();
		}

		private void OnAfterEntityCreated(Entity entity)
		{
			_monoEntities.CreateMonoEntity(entity);
		}

		private void OnBeforeEntityDestroyed(int entityId)
		{
			_monoEntities.DestroyMonoEntity(entityId);
		}

		private void OnAfterViewAssigned(int entityId)
		{
			_entityViewSynchronizer.SynchronizeView(entityId);
		}

		private void OnBeforeViewUnassigned(int entityId)
		{
			_entityViewSynchronizer.DestroyView(entityId);
		}

		public void OnAfterAssigned<TMonoComponent>(int entityId) where TMonoComponent : MonoComponent
		{
			var monoEntities = _monoEntities.Set;
			if (!monoEntities.IsAssigned(entityId))
			{
				return;
			}

			var go = monoEntities.Get(entityId).gameObject;

			if (!go.TryGetComponent<TMonoComponent>(out var component))
			{
				component = go.AddComponent<TMonoComponent>();
				component.Synchronize(_registry, _registry.GetEntity(entityId));
			}
		}

		public void OnBeforeUnassigned<TMonoComponent>(int entityId) where TMonoComponent : MonoComponent
		{
			var monoEntities = _monoEntities.Set;
			if (!monoEntities.IsAssigned(entityId))
			{
				return;
			}

			var go = monoEntities.Get(entityId).gameObject;

			if (go.TryGetComponent<TMonoComponent>(out var component))
			{
				Object.Destroy(component);
			}
		}

		public void Dispose()
		{
			UnsubscribeEntities();
			UnsubscribeViews();
		}
	}
}
