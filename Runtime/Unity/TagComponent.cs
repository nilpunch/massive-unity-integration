using UnityEngine;

namespace Massive.Unity
{
	[RequireComponent(typeof(MonoEntity))]
	[DisallowMultipleComponent]
	public class TagComponent<TComponent> : MonoComponent
	{
		private Registry _registry;
		private Entity _entity;

		public override void ApplyToEntity(Registry registry, Entity entity)
		{
			registry.Assign<TComponent>(entity);
		}
	}
}
