namespace Massive.Unity
{
	public class TransformComponent : UnmanagedComponentBase<LocalTransform, TransformComponent>
	{
		private LocalTransform _lastRegistryLocalTransform;
		private LocalTransform _lastGoLocalTransform;

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

			ApplyTransformData(_registry.Get<LocalTransform>(entity));
		}

		private void OnDestroy()
		{
			if (_registry != null)
			{
				_registry.Unassign<LocalTransform>(_entity);
			}
		}

		private void LateUpdate()
		{
			if (_registry == null)
			{
				return;
			}

			var goTransformData = GetTransformData();
			ref var registryTransformData = ref _registry.Get<LocalTransform>(_entity);
			if (_lastRegistryLocalTransform != registryTransformData)
			{
				_lastRegistryLocalTransform = registryTransformData;
				ApplyTransformData(registryTransformData);
			}
			else if (_lastGoLocalTransform != goTransformData)
			{
				_lastRegistryLocalTransform = goTransformData;
				_lastGoLocalTransform = goTransformData;
				registryTransformData = goTransformData;
			}
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
