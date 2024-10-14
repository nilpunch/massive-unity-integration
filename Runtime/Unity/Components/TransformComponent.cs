using UnityEngine;

namespace Massive.Unity
{
	public class TransformComponent : UnmanagedComponentBase<LocalTransform, TransformComponent>
	{
		private LocalTransform _lastGoLocalTransform;

		private Registry _registry;
		private Entity _entity;

		public override void ApplyToEntity(Registry registry, Entity entity)
		{
			registry.Assign(entity, GetTransformData());
		}

		public override void Synchronize(Registry registry, Entity entity)
		{
			_registry = registry;
			_entity = entity;

			ApplyTransformData(_registry.Get<LocalTransform>(entity));
		}

		public override void UnassignComponent()
		{
			if (_registry != null)
			{
				_registry.Unassign<LocalTransform>(_entity);
			}
		}

		private void LateUpdate()
		{
			if (_registry == null || !_registry.IsAlive(_entity))
			{
				return;
			}

			var lastGoLocalTransform = _lastGoLocalTransform;
			var goLocalTransform = GetTransformData();

			Vector3 deltaPosition = goLocalTransform.Position - lastGoLocalTransform.Position;
			Quaternion deltaRotation = Quaternion.Inverse(lastGoLocalTransform.Rotation) * goLocalTransform.Rotation;
			Vector3 deltaScale = goLocalTransform.Scale - lastGoLocalTransform.Scale;

			ref var registryTransformData = ref _registry.Get<LocalTransform>(_entity);

			registryTransformData.Position += deltaPosition;
			registryTransformData.Rotation *= deltaRotation.normalized;
			registryTransformData.Scale += deltaScale;

			ApplyTransformData(registryTransformData);
		}

		private LocalTransform GetTransformData()
		{
			return new LocalTransform()
			{
				Position = transform.localPosition,
				Rotation = transform.localRotation,
				Scale = transform.localScale,
			};
		}

		private void ApplyTransformData(LocalTransform localTransformData)
		{
			transform.localPosition = localTransformData.Position;
			transform.localRotation = localTransformData.Rotation;
			transform.localScale = localTransformData.Scale;
			_lastGoLocalTransform = localTransformData;
		}
	}
}
