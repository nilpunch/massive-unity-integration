using UnityEngine;

namespace Massive.Unity
{
	public class ViewComponent : MonoComponent
	{
		[SerializeField] private EntityView _viewPrefab;

		private Registry _registry;
		private Entity _entity;

		public override void ApplyToEntity(Registry registry, Entity entity)
		{
			registry.Assign(entity, _viewPrefab.ViewAsset);
		}
	}
}
