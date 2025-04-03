using System;

namespace Massive.Unity
{
	public class UnityEntitySynchronization : IDisposable
	{
		private readonly World _world;
		private readonly EntityViewSynchronizer _entityViewSynchronizer;

		public UnityEntitySynchronization(World world, EntityViewPool entityViewPool)
		{
			_world = world;
			_entityViewSynchronizer = new EntityViewSynchronizer(world, entityViewPool);
		}

		public void SubscribeViews()
		{
			_world.SparseSet<ViewAsset>().AfterAdded += OnAfterViewAdded;
			_world.SparseSet<ViewAsset>().BeforeRemoved += OnBeforeViewRemoved;
		}

		public void UnsubscribeViews()
		{
			_world.SparseSet<ViewAsset>().AfterAdded -= OnAfterViewAdded;
			_world.SparseSet<ViewAsset>().BeforeRemoved -= OnBeforeViewRemoved;
		}

		public void SynchronizeViews()
		{
			_entityViewSynchronizer.SynchronizeAll();
		}

		private void OnAfterViewAdded(int entityId)
		{
			_entityViewSynchronizer.SynchronizeView(entityId);
		}

		private void OnBeforeViewRemoved(int entityId)
		{
			_entityViewSynchronizer.DestroyView(entityId);
		}

		public void Dispose()
		{
			UnsubscribeViews();
		}
	}
}
