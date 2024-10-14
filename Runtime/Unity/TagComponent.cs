using UnityEngine;

namespace Massive.Unity
{
	[RequireComponent(typeof(MonoEntity))]
	[DisallowMultipleComponent]
	public class TagComponent<TComponent, TMonoComponent> : UnmanagedComponentBase<TComponent, TMonoComponent>
		where TComponent : unmanaged
		where TMonoComponent : TagComponent<TComponent, TMonoComponent>
	{
		private Registry _registry;
		private Entity _entity;

		public override void ApplyToEntity(Registry registry, Entity entity)
		{
			registry.Assign<TComponent>(entity);
		}

		public override void Synchronize(Registry registry, Entity entity)
		{
			_entity = entity;
			_registry = registry;
		}

		public override void UnassignComponent()
		{
			if (_registry != null)
			{
				_registry.Unassign<TComponent>(_entity);
			}
		}
	}
}
