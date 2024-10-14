namespace Massive.Unity
{
	public class EntityViewSynchronizer
	{
		private readonly DataSet<ViewInstance> _viewInstances = new DataSet<ViewInstance>();
		private readonly Registry _registry;
		private readonly EntityViewPool _viewPool;

		public EntityViewSynchronizer(Registry registry, EntityViewPool viewPool)
		{
			_registry = registry;
			_viewPool = viewPool;
		}

		public void SynchronizeAll()
		{
			var viewAssets = _registry.DataSet<ViewAsset>();

			// Remove to pool all invalid views
			var monoViewsData = _viewInstances.Data;
			var monoViewsIds = _viewInstances.Ids;
			for (int i = _viewInstances.Count - 1; i >= 0; i--)
			{
				int entityId = monoViewsIds[i];
				var viewInstance = monoViewsData[i];
				if (!viewAssets.IsAssigned(entityId) || !viewAssets.Get(entityId).Equals(viewInstance.Asset))
				{
					UnassignViewInstance(entityId, viewInstance);
				}
			}

			// Add whats missing
			var viewAssetData = viewAssets.Data;
			var viewAssetIds = viewAssets.Ids;
			for (int i = 0; i < viewAssets.Count; i++)
			{
				int entityId = viewAssetIds[i];
				var viewAsset = viewAssetData[i];

				if (!_viewInstances.IsAssigned(entityId))
				{
					AssignViewInstance(viewAsset, entityId);
				}
			}
		}

		public void SynchronizeView(int entityId)
		{
			var viewAsset = _registry.Get<ViewAsset>(entityId);

			if (_viewInstances.IsAssigned(entityId))
			{
				var viewInstance = _viewInstances.Get(entityId);

				// If instance is alright, nothing to be done
				if (viewInstance.Asset.Equals(viewAsset))
				{
					return;
				}
				else
				{
					UnassignViewInstance(entityId, viewInstance);
				}
			}

			AssignViewInstance(viewAsset, entityId);
		}

		public void DestroyView(int entityId)
		{
			if (_viewInstances.IsAssigned(entityId))
			{
				UnassignViewInstance(entityId, _viewInstances.Get(entityId));
			}
		}

		private void AssignViewInstance(ViewAsset viewAsset, int entityId)
		{
			var view = _viewPool.CreateView(viewAsset);

			view.transform.SetParent(null);
			view.AssignEntity(_registry, _registry.GetEntity(entityId));

			_viewInstances.Assign(entityId, new ViewInstance() { Instance = view, Asset = viewAsset });
		}

		private void UnassignViewInstance(int entityId, ViewInstance viewInstance)
		{
			viewInstance.Instance.UnassignEntity();

			_viewPool.ReturnView(viewInstance.Instance);

			_viewInstances.Unassign(entityId);
		}
	}
}
