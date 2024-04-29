using Massive;
using UnityEngine;

namespace UPR
{
	[RequireComponent(typeof(MonoEntity))]
	[DisallowMultipleComponent]
	public class ManagedComponent<TComponent, TMonoComponent> : ManagedComponentBase<TComponent, TMonoComponent>
		where TComponent : IManaged<TComponent>
		where TMonoComponent : ManagedComponent<TComponent, TMonoComponent>
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
			registry.Get<TComponent>(entity).CopyTo(ref _data);
		}

		private void OnDestroy()
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
			if (_registry != null)
			{
				_registry.Get<TComponent>(_entity).CopyTo(ref _data);
			}
		}
	}
}
