using UnityEngine;

namespace Massive.Unity
{
	public class ViewProvider : ComponentProvider
	{
		[SerializeField] private EntityView _viewPrefab;

		private Registry _registry;
		private Entity _entity;

		public override void ApplyToEntity(ServiceLocator serviceLocator, Entity entity)
		{
			var viewAsset = serviceLocator.Find<ViewDataBase>().GetViewAsset(_viewPrefab);
			serviceLocator.Find<Registry>().Assign(entity, viewAsset);
		}
	}
}
