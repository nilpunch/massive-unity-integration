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
			registry.Get<TComponent>(entityId).CopyTo(ref _data);
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
				_data.CopyTo(ref _registry.Get<TComponent>(_entityId));
			}
		}

		private void LateUpdate()
		{
			if (_registry != null)
			{
				_registry.Get<TComponent>(_entityId).CopyTo(ref _data);
			}
		}
	}
}
