using UnityEngine;

namespace Massive.Unity
{
	public class ViewComponent : UnmanagedComponentBase<ViewAsset, ViewComponent>
	{
		[SerializeField] private GameObject _viewPrefab;
		[SerializeField] private ViewDataBaseConfig _viewConfig;

		private IRegistry _registry;
		private int _entityId;

		public override void ApplyToEntity(IRegistry registry, Entity entity)
		{
			registry.Assign(entity, _viewConfig.GetAssetId(_viewPrefab));
		}

		public override void Synchronize(IRegistry registry, int entityId)
		{
			_entityId = entityId;
			_registry = registry;
		}

		private void OnDestroy()
		{
			if (_registry != null)
			{
				_registry.Unassign<ViewAsset>(_entityId);
			}
		}
	}
}