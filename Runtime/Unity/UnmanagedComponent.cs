using UnityEngine;

namespace Massive.Unity
{
	[RequireComponent(typeof(MonoEntity))]
	[DisallowMultipleComponent]
	public class UnmanagedComponent<TComponent, TMonoComponent> : UnmanagedComponentBase<TComponent, TMonoComponent>
		where TComponent : unmanaged
		where TMonoComponent : UnmanagedComponent<TComponent, TMonoComponent>
	{
		[SerializeField] private TComponent _data;

		private IRegistry _registry;
		private int _entityId;

		public override void ApplyToEntity(IRegistry registry, Entity entity)
		{
			registry.Assign(entity, _data);
		}

		public override void Synchronize(IRegistry registry, int entityId)
		{
			_registry = registry;
			_entityId = entityId;
			_data = registry.Get<TComponent>(entityId);
		}

		private void OnDestroy()
		{
			if (_registry != null)
			{
				_registry.Unassign<TComponent>(_entityId);
			}
		}

		private void OnValidate()
		{
			if (_registry != null)
			{
				_registry.Get<TComponent>(_entityId) = _data;
			}
		}

		private void LateUpdate()
		{
			if (_registry != null)
			{
				_data = _registry.Get<TComponent>(_entityId);
			}
		}
	}
}
