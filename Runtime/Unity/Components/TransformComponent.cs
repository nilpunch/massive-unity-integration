using Massive;

namespace Massive.Unity
{
	public class TransformComponent : UnmanagedComponentBase<Transform, TransformComponent>
	{
		private Transform _lastRegistryTransform;
		private Transform _lastGOTransform;

		private IRegistry _registry;
		private Entity _entity;

		public override void ApplyToEntity(IRegistry registry, Entity entity)
		{
			registry.Assign(entity, GetTransformData());
		}

		public override void Synchronize(IRegistry registry, Entity entity)
		{
			_registry = registry;
			_entity = entity;

			var transformData = _registry.Get<Transform>(entity);

			transform.localPosition = transformData.LocalPosition;
			transform.localRotation = transformData.LocalRotation;
			transform.localScale = transformData.LocalScale;
		}

		private void OnDestroy()
		{
			if (_registry != null)
			{
				_registry.Unassign<Transform>(_entity);
			}
		}

		private void LateUpdate()
		{
			if (_registry == null)
			{
				return;
			}

			var goTransformData = GetTransformData();
			ref var registryTransformData = ref _registry.Get<Transform>(_entity);
			if (_lastRegistryTransform != registryTransformData)
			{
				_lastRegistryTransform = registryTransformData;
				_lastGOTransform = registryTransformData;
				ApplyTransformData(registryTransformData);
			}
			else if (_lastGOTransform != goTransformData)
			{
				_lastRegistryTransform = goTransformData;
				_lastGOTransform = goTransformData;
				registryTransformData = goTransformData;
			}
		}

		private Transform GetTransformData()
		{
			return new Transform()
			{
				LocalPosition = transform.localPosition,
				LocalRotation = transform.localRotation,
				LocalScale = transform.localScale,
			};
		}

		private void ApplyTransformData(Transform transformData)
		{
			transform.localPosition = transformData.LocalPosition;
			transform.localRotation = transformData.LocalRotation;
			transform.localScale = transformData.LocalScale;
		}
	}
}
