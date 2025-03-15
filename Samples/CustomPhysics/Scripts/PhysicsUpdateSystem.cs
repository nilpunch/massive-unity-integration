using Massive.Physics;
using Mathematics.Fixed;
using UnityEngine;

namespace Massive.Unity.Samples.Physics
{
	public class PhysicsUpdateSystem : UpdateSystem
	{
		[SerializeField] private int _substeps = 20;
		[SerializeField] private FVector3 _gravity = new FVector3(0f.ToFP(), -9.8f.ToFP(), 0f.ToFP());

		private Registry _registry;

		public override void Init(ServiceLocator serviceLocator)
		{
			_registry = serviceLocator.Find<Registry>();
		}

		public override void UpdateFrame(FP deltaTime)
		{
			var subDeltaTime = deltaTime / _substeps;

			for (var i = 0; i < _substeps; i++)
			{
				Simulation.IntegrateVelocities(_registry, subDeltaTime, _gravity);
				Simulation.IntegratePositions(_registry, subDeltaTime);
			}

			_registry.View().ForEach(static (ref LocalTransform transform, ref Body body) =>
			{
				transform.Position = new Vector3(
					body.Center.X.ToFloat(),
					body.Center.Y.ToFloat(),
					body.Center.Z.ToFloat());
				transform.Rotation = new Quaternion(
					body.Rotation.X.ToFloat(),
					body.Rotation.Y.ToFloat(),
					body.Rotation.Z.ToFloat(),
					body.Rotation.W.ToFloat());
			});
		}
	}
}
