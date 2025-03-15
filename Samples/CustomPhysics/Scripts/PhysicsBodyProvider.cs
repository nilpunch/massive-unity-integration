using Massive.Physics;
using Mathematics.Fixed;
using UnityEngine;

namespace Massive.Unity.Samples.Physics
{
	public class PhysicsBodyProvider : ComponentProvider
	{
		[SerializeField] private FP _radius = FP.One;
		[SerializeField] private FP _density = FP.One;
		[SerializeField] private FVector3 _impulsePosition = FVector3.Zero;
		[SerializeField] private FVector3 _impulse = FVector3.Zero;

		public override void ApplyToEntity(ServiceLocator serviceLocator, Entity entity)
		{
			var mass = MassUtils.SphereMass(_radius, _density);

			var body = Body.Create(
				transform.position.ToFVector3(),
				transform.rotation.ToFQuaternion(),
				mass,
				MassUtils.SphereInertia(_radius, mass));

			body.ApplyLinearImpulse(_impulse, body.Position + _impulsePosition);

			serviceLocator.Find<Registry>().Assign(entity, body);
		}
	}
}
