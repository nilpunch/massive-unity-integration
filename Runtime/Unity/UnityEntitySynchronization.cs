using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Massive.Unity
{
	public class UnityEntitySynchronization : IComponentsEventHandler, IDisposable
	{
		private readonly IRegistry _registry;
		private readonly ViewDataBase _viewDataBase;
		private readonly MonoEntitiesDataBase _monoEntities;
		private int _entityCounter;

		public UnityEntitySynchronization(IRegistry registry, ViewDataBase viewDataBase)
		{
			_registry = registry;
			_viewDataBase = viewDataBase;
			_monoEntities = new MonoEntitiesDataBase(registry);

			_registry.Entities.AfterCreated += OnAfterEntityCreated;
			_registry.Entities.BeforeDestroyed += OnBeforeEntityDestroyed;

			foreach (var reflector in ComponentReflectors.All)
			{
				reflector.SubscribeAssignCallbacks(_registry, this);
			}

			SyncronizeEntities();
			SynchronizeComponents();

			_registry.Any<ViewAsset>().AfterAssigned += OnAfterViewAssigned;
			_registry.Any<ViewAsset>().BeforeUnassigned += OnBeforeViewUnassigned;

			foreach (var entityId in registry.Any<ViewAsset>().Ids)
			{
				OnAfterViewAssigned(entityId);
			}
		}

		public void SyncronizeEntities()
		{
			var monoEntities = _monoEntities.Set;
			foreach (var entityId in monoEntities.Ids)
			{
				if (!_registry.IsAlive(entityId))
				{
					_monoEntities.DestroyMonoEntity(entityId);
				}
			}

			foreach (var entity in _registry.Entities.Alive)
			{
				if (!monoEntities.IsAssigned(entity.Id))
				{
					_monoEntities.CreateMonoEntity(entity);
				}
			}
		}

		public void SynchronizeComponents()
		{
			foreach (var reflector in ComponentReflectors.All)
			{
				reflector.SynchronizeComponents(_registry, this);
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

		private void OnAfterViewAssigned(int entityId)
		{
			var viewId = _registry.Get<ViewAsset>(entityId);
			var view = _viewDataBase.GetView(viewId);

			view.transform.SetParent(_monoEntities.Set.Get(entityId).transform);
			view.transform.localPosition = Vector3.zero;
			view.transform.localRotation = Quaternion.identity;
			view.transform.localScale = Vector3.one;
		}

		private void OnBeforeViewUnassigned(int entityId)
		{
			var transform = _monoEntities.Set.Get(entityId).transform;

			if (transform.childCount != 0)
			{
				var view = transform.GetChild(0).gameObject;
				_viewDataBase.ReturnView(view);
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
