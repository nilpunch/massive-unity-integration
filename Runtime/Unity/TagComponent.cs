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
		private int _entityId;

		public override void ApplyToEntity(IRegistry registry, Entity entity)
		{
			registry.Assign<TComponent>(entity);
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
				_registry.Unassign<TComponent>(_entityId);
			}
		}
	}
}
