namespace Massive.Unity
{
	public class LocalTransformProvider : ComponentProvider
	{
		public override void ApplyToEntity(ServiceLocator serviceLocator, Entity entity)
		{
			serviceLocator.Find<World>().Set(entity, GetTransformData());
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
	}
}
