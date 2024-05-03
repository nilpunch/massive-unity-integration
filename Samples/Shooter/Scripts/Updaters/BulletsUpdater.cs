using Massive.Unity;

namespace Massive.Samples.Shooter
{
	public class BulletsUpdater : UpdateSystem
	{
		private IRegistry _registry;
		private GroupView<BulletState, LocalTransform> _bullets;

		public override void Init(IRegistry registry)
		{
			_registry = registry;
			_bullets = new GroupView<BulletState, LocalTransform>(registry, registry.Group(_registry.Many<BulletState>(), _registry.Many<LocalTransform>()));
		}

		public override void UpdateFrame(float deltaTime)
		{
			_bullets.ForEachExtra((_registry, deltaTime), (int entityId, ref BulletState bullet, ref LocalTransform bulletTransform,
				(IRegistry Registry, float DeltaTime) inner) =>
			{
				bullet.Lifetime -= inner.DeltaTime;
				if (bullet.IsDestroyed)
				{
					inner.Registry.Destroy(entityId);
					return;
				}

				bulletTransform.Position += bullet.Velocity * inner.DeltaTime;
			});
		}
	}
}
