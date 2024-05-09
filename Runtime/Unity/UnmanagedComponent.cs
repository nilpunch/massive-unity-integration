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
		private Entity _entity;

		public override void ApplyToEntity(IRegistry registry, Entity entity)
		{
			registry.Assign(entity, _data);
		}

		public override void Synchronize(IRegistry registry, Entity entity)
		{
			_registry = registry;
			_entity = entity;
			_data = registry.Get<TComponent>(entity);
		}

		public override void UnassignComponent()
		{
			if (_registry != null)
			{
				_registry.Unassign<TComponent>(_entity);
			}
		}

		private void OnValidate()
		{
			if (_registry != null)
			{
				_registry.Get<TComponent>(_entity) = _data;
			}
		}

		private void LateUpdate()
		{
			if (_registry != null && _registry.IsAlive(_entity))
			{
				_data = _registry.Get<TComponent>(_entity);
			}
		}
	}
}
