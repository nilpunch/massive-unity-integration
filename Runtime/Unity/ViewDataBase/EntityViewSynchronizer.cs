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
			foreach (var (pageIndex, pageLength, indexOffset) in new PageSequence(monoViewsData.PageSize, _viewInstances.Count))
			{
				var page = monoViewsData.Pages[pageIndex];
				for (var index = pageLength - 1; index >= 0; index--)
				{
					if (indexOffset + index > _viewInstances.Count)
					{
						index = _viewInstances.Count - indexOffset;
						continue;
					}

					var id = _viewInstances.Packed[indexOffset + index];
					var viewInstance = page[index];
					if (!viewAssets.IsAssigned(id) || !viewAssets.Get(id).Equals(viewInstance.Asset))
					{
						UnassignViewInstance(id, viewInstance);
					}
				}
			}

			// Add whats missing
			var viewAssetData = viewAssets.Data;
			foreach (var (pageIndex, pageLength, indexOffset) in new PageSequence(viewAssetData.PageSize, viewAssets.Count))
			{
				var page = viewAssetData.Pages[pageIndex];
				for (var index = pageLength - 1; index >= 0; index--)
				{
					if (indexOffset + index > viewAssets.Count)
					{
						index = viewAssets.Count - indexOffset;
						continue;
					}

					var id = viewAssets.Packed[indexOffset + index];
					var viewAsset = page[index];
					if (!_viewInstances.IsAssigned(id))
					{
						AssignViewInstance(viewAsset, id);
					}
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
