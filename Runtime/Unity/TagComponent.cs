using UnityEngine;

namespace Massive.Unity
{
	[RequireComponent(typeof(MonoEntity))]
	[DisallowMultipleComponent]
	public class TagComponent<TComponent, TMonoComponent> : UnmanagedComponentBase<TComponent, TMonoComponent>
		where TComponent : unmanaged
		where TMonoComponent : TagComponent<TComponent, TMonoComponent>
	{
		private IRegistry _registry;
		private Entity _entity;

		public override void ApplyToEntity(IRegistry registry, Entity entity)
		{
			registry.Assign<TComponent>(entity);
		}

		public override void Synchronize(IRegistry registry, Entity entity)
		{
			_entity = entity;
			_registry = registry;
		}

		public override void DestroyComponent()
		{
			if (_registry != null)
			{
				_registry.Unassign<TComponent>(_entity);
			}
		}
	}
}
