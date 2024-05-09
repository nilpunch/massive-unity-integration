using UnityEngine;

namespace Massive.Unity
{
	public class ViewComponent : UnmanagedComponentBase<ViewAsset, ViewComponent>
	{
		[SerializeField] private EntityView _viewPrefab;
		[SerializeField] private ViewDataBaseConfig _viewConfig;

		private IRegistry _registry;
		private Entity _entity;

		public override void ApplyToEntity(IRegistry registry, Entity entity)
		{
			registry.Assign(entity, _viewConfig.GetAssetId(_viewPrefab));
		}

		public override void Synchronize(IRegistry registry, Entity entity)
		{
			_entity = entity;
			_registry = registry;
		}

		public override void UnassignComponent()
		{
			if (_registry != null)
			{
				_registry.Unassign<ViewAsset>(_entity);
			}
		}
	}
}
