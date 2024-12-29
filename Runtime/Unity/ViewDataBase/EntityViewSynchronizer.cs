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
			foreach (var page in new PageSequence(monoViewsData.PageSize, _viewInstances.Count))
			{
				var dataPage = monoViewsData.Pages[page.Index];
				for (var index = page.Length - 1; index >= 0; index--)
				{
					if (page.Offset + index > _viewInstances.Count)
					{
						index = _viewInstances.Count - page.Offset;
						continue;
					}

					var id = _viewInstances.Packed[page.Offset + index];
					var viewInstance = dataPage[index];
					if (!viewAssets.IsAssigned(id) || !viewAssets.Get(id).Equals(viewInstance.Asset))
					{
						UnassignViewInstance(id, viewInstance);
					}
				}
			}

			// Add whats missing
			var viewAssetData = viewAssets.Data;
			foreach (var page in new PageSequence(viewAssetData.PageSize, viewAssets.Count))
			{
				var dataPage = viewAssetData.Pages[page.Index];
				for (var index = page.Length - 1; index >= 0; index--)
				{
					if (page.Offset + index > viewAssets.Count)
					{
						index = viewAssets.Count - page.Offset;
						continue;
					}

					var id = viewAssets.Packed[page.Offset + index];
					var viewAsset = dataPage[index];
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
