using System;

namespace Massive.Unity
{
	public class UnityEntitySynchronization : IDisposable
	{
		private readonly Registry _registry;
		private readonly EntityViewSynchronizer _entityViewSynchronizer;

		public UnityEntitySynchronization(Registry registry, EntityViewPool entityViewPool)
		{
			_registry = registry;
			_entityViewSynchronizer = new EntityViewSynchronizer(registry, entityViewPool);
		}

		public void SubscribeViews()
		{
			_registry.Set<ViewAsset>().AfterAssigned += OnAfterViewAssigned;
			_registry.Set<ViewAsset>().BeforeUnassigned += OnBeforeViewUnassigned;
		}

		public void UnsubscribeViews()
		{
			_registry.Set<ViewAsset>().AfterAssigned -= OnAfterViewAssigned;
			_registry.Set<ViewAsset>().BeforeUnassigned -= OnBeforeViewUnassigned;
		}

		public void SynchronizeViews()
		{
			_entityViewSynchronizer.SynchronizeAll();
		}

		private void OnAfterViewAssigned(int entityId)
		{
			_entityViewSynchronizer.SynchronizeView(entityId);
		}

		private void OnBeforeViewUnassigned(int entityId)
		{
			_entityViewSynchronizer.DestroyView(entityId);
		}

		public void Dispose()
		{
			UnsubscribeViews();
		}
	}
}
