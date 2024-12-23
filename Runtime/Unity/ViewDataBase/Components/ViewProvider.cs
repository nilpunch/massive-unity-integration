using UnityEngine;

namespace Massive.Unity
{
	public class ViewProvider : ComponentProvider
	{
		[SerializeField] private EntityView _viewPrefab;

		private Registry _registry;
		private Entity _entity;

		public override void ApplyToEntity(Registry registry, Entity entity)
		{
			var viewAsset = registry.Service<ViewDataBase>().GetViewAsset(_viewPrefab);
			registry.Assign(entity, viewAsset);
		}
	}
}
