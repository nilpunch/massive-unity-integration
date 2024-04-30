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

			for (var i = 0; i < _registry.Entities.Alive.Length; i++)
			{
				OnAfterEntityCreated(_registry.Entities.Alive[i]);
			}

			_registry.Entities.AfterCreated += OnAfterEntityCreated;
			_registry.Entities.BeforeDestroyed += OnBeforeEntityDestroyed;

			foreach (var reflector in ComponentReflectors.All)
			{
				reflector.SubscribeAssignCallbacks(_registry, this);
			}

			foreach (var reflector in ComponentReflectors.All)
			{
				reflector.SynchronizeGameObjects(_registry, this);
			}

			_registry.Any<ViewAsset>().AfterAssigned += OnAfterViewAssigned;
			_registry.Any<ViewAsset>().BeforeUnassigned += OnBeforeViewUnassigned;

			foreach (var entityId in registry.Any<ViewAsset>().Ids)
			{
				OnAfterViewAssigned(entityId);
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
			_monoEntities.MonoEntities.Get(entityId).gameObject.AddComponent<TMonoComponent>().Synchronize(_registry, _registry.GetEntity(entityId));
		}

		public void OnBeforeUnassigned<TMonoComponent>(int entityId) where TMonoComponent : MonoComponent
		{
			Object.Destroy(_monoEntities.MonoEntities.Get(entityId).gameObject.GetComponent<TMonoComponent>());
		}

		private void OnAfterViewAssigned(int entityId)
		{
			var viewId = _registry.Get<ViewAsset>(entityId);
			var view = _viewDataBase.GetView(viewId);

			view.transform.SetParent(_monoEntities.MonoEntities.Get(entityId).transform);
			view.transform.localPosition = Vector3.zero;
			view.transform.localRotation = Quaternion.identity;
			view.transform.localScale = Vector3.one;
		}

		private void OnBeforeViewUnassigned(int entityId)
		{
			var transform = _monoEntities.MonoEntities.Get(entityId).transform;

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
