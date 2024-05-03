using System;
using Object = UnityEngine.Object;

namespace Massive.Unity
{
	public class UnityEntitySynchronization : IComponentsEventHandler, IDisposable
	{
		private readonly IRegistry _registry;
		private readonly EntityViewSynchronizer _entityViewSynchronizer;
		private readonly MonoEntitySynchronizer _monoEntities;

		public UnityEntitySynchronization(IRegistry registry, EntityViewPool entityViewPool, bool reactiveSynchronization = true)
		{
			_registry = registry;
			_entityViewSynchronizer = new EntityViewSynchronizer(registry, entityViewPool);
			_monoEntities = new MonoEntitySynchronizer(registry);

			if (!reactiveSynchronization)
			{
				return;
			}

			_registry.Entities.AfterCreated += OnAfterEntityCreated;
			_registry.Entities.BeforeDestroyed += OnBeforeEntityDestroyed;

			_registry.Any<ViewAsset>().AfterAssigned += OnAfterViewAssigned;
			_registry.Any<ViewAsset>().BeforeUnassigned += OnBeforeViewUnassigned;

			foreach (var reflector in ComponentReflectors.All)
			{
				reflector.SubscribeAssignCallbacks(_registry, this);
			}
		}

		public void SynchronizeEntities()
		{
			_monoEntities.SynchronizeAll();
		}

		public void SynchronizeViews()
		{
			_entityViewSynchronizer.SynchronizeAll();
		}

		public void SynchronizeComponents()
		{
			foreach (var reflector in ComponentReflectors.All)
			{
				reflector.SynchronizeComponents(_registry, _monoEntities.Set, this);
			}
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
			}

			component.Synchronize(_registry, _registry.GetEntity(entityId));
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
			_registry.Entities.AfterCreated -= OnAfterEntityCreated;
			_registry.Entities.BeforeDestroyed -= OnBeforeEntityDestroyed;

			_registry.Any<ViewAsset>().AfterAssigned -= OnAfterViewAssigned;
			_registry.Any<ViewAsset>().BeforeUnassigned -= OnBeforeViewUnassigned;

			foreach (var reflector in ComponentReflectors.All)
			{
				reflector.UnsubscribeAssignCallbacks(_registry, this);
			}
		}
	}
}
