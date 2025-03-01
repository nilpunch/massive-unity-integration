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

		public override void UpdateFrame(float deltaTime)
		{
			var substeppedDeltaTime = deltaTime.ToFP() / _substeps;

			for (var i = 0; i < _substeps; i++)
			{
				Simulation.Integrate(_registry, substeppedDeltaTime, _gravity);
				Simulation.SolveDistanceConstraints(_registry, substeppedDeltaTime);
				Simulation.UpdateVelocities(_registry, substeppedDeltaTime);
			}

			_registry.View().ForEach(static (ref LocalTransform transform, ref Body body) =>
			{
				transform.Position = new Vector3(
					body.Position.X.ToFloat(),
					body.Position.Y.ToFloat(),
					body.Position.Z.ToFloat());
				transform.Rotation = new Quaternion(
					body.Rotation.X.ToFloat(),
					body.Rotation.Y.ToFloat(),
					body.Rotation.Z.ToFloat(),
					body.Rotation.W.ToFloat());
			});
		}
	}
}
