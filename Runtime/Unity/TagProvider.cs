using UnityEngine;

namespace Massive.Unity
{
	[RequireComponent(typeof(EntityProvider))]
	[DisallowMultipleComponent]
	public class TagProvider<TComponent> : ComponentProvider
	{
		private Registry _registry;
		private Entity _entity;

		public override void ApplyToEntity(Registry registry, Entity entity)
		{
			registry.Assign<TComponent>(entity);
		}
	}
}
