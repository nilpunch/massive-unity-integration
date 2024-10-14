using UnityEngine;

namespace Massive.Unity
{
	[RequireComponent(typeof(MonoEntity))]
	[DisallowMultipleComponent]
	public class ManagedComponent<TComponent, TMonoComponent> : ManagedComponentBase<TComponent, TMonoComponent>
		where TComponent : IManaged<TComponent>
		where TMonoComponent : ManagedComponent<TComponent, TMonoComponent>
	{
		[SerializeField] private TComponent _data;

		private Registry _registry;
		private Entity _entity;

		public override void ApplyToEntity(Registry registry, Entity entity)
		{
			registry.Assign(entity, _data);
		}

		public override void Synchronize(Registry registry, Entity entity)
		{
			_registry = registry;
			_entity = entity;
			registry.Get<TComponent>(entity).CopyTo(ref _data);
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
				_data.CopyTo(ref _registry.Get<TComponent>(_entity));
			}
		}

		private void LateUpdate()
		{
			if (_registry != null && _registry.IsAlive(_entity))
			{
				_registry.Get<TComponent>(_entity).CopyTo(ref _data);
			}
		}
	}
}
