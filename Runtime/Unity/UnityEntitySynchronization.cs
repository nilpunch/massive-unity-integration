using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Massive.Unity
{
	public class UnityEntitySynchronization : IComponentsEventHandler, IDisposable
	{
		private readonly IRegistry _registry;
		private readonly ViewPool _viewPool;
		private readonly MonoEntitiesDataBase _monoEntities;
		private readonly DataSet<ViewInstance> _monoViews;
		private int _entityCounter;

		public UnityEntitySynchronization(IRegistry registry, ViewPool viewPool, bool reactiveSynchronization = true)
		{
			_registry = registry;
			_viewPool = viewPool;
			_monoEntities = new MonoEntitiesDataBase(registry);
			_monoViews = new DataSet<ViewInstance>();

			SyncronizeEntities();
			SynchronizeComponents();
			SynchronizeViews();

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

		public void SynchronizeViews()
		{
			var viewAssets = _registry.Components<ViewAsset>();

			// Remove to pool all invalid views
			var monoViewsData = _monoViews.Data;
			var monoViewsIds = _monoViews.Ids;
			for (int i = monoViewsIds.Length - 1; i >= 0; i--)
			{
				int entityId = monoViewsIds[i];
				var viewInstance = monoViewsData[i];
				if (!viewAssets.IsAssigned(entityId) || !viewAssets.Get(entityId).Equals(viewInstance.Asset))
				{
					_viewPool.ReturnView(viewInstance.Instance);
					_monoViews.Unassign(entityId);
				}
			}

			// Add whats missing
			var viewAssetData = viewAssets.Data;
			var viewAssetIds = viewAssets.Ids;
			for (int i = 0; i < viewAssetIds.Length; i++)
			{
				int entityId = viewAssetIds[i];
				var viewAsset = viewAssetData[i];

				if (!_monoViews.IsAssigned(entityId))
				{
					var view = _viewPool.GetView(viewAsset);

					_monoViews.Assign(entityId, new ViewInstance() { Instance = view, Asset = viewAsset });

					view.transform.SetParent(_monoEntities.Set.Get(entityId).transform);
					view.transform.localPosition = Vector3.zero;
					view.transform.localRotation = Quaternion.identity;
					view.transform.localScale = Vector3.one;
				}
			}
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
			var viewAsset = _registry.Get<ViewAsset>(entityId);

			if (_monoViews.IsAssigned(entityId))
			{
				var viewInstance = _monoViews.Get(entityId);

				// If instance is alright, nothing to be done
				if (viewInstance.Asset.Equals(viewAsset))
				{
					return;
				}
				else
				{
					_viewPool.ReturnView(viewInstance.Instance);
					_monoViews.Unassign(entityId);
				}
			}

			var view = _viewPool.GetView(viewAsset);

			_monoViews.Assign(entityId, new ViewInstance() { Instance = view, Asset = viewAsset });

			view.transform.SetParent(_monoEntities.Set.Get(entityId).transform);
			view.transform.localPosition = Vector3.zero;
			view.transform.localRotation = Quaternion.identity;
			view.transform.localScale = Vector3.one;
		}

		private void OnBeforeViewUnassigned(int entityId)
		{
			if (_monoViews.IsAssigned(entityId))
			{
				_viewPool.ReturnView(_monoViews.Get(entityId).Instance);
				_monoViews.Unassign(entityId);
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