using Mathematics.Fixed;
using UnityEngine;

namespace Massive.Unity.Samples.Physics
{
	public class PhysicsBodyProvider : ComponentProvider
	{
		[SerializeField] private FP _radius = 1.ToFP();
		[SerializeField] private FP _density = 1.ToFP();
		[SerializeField] private FVector3 _initialAngularVelocity = FVector3.Zero;
		[SerializeField] private FVector3 _initialLinearVelocity = FVector3.Zero;

		public override void ApplyToEntity(ServiceLocator serviceLocator, Entity entity)
		{
			var mass = (4f / 3f).ToFP() * FP.Pi * _radius * _radius * _radius * _density;
			var invMass = FP.One / mass;
			var inertia = 2.ToFP() / 5.ToFP() * mass * _radius * _radius;
			var position = new FVector3(
				transform.position.x.ToFP(),
				transform.position.y.ToFP(),
				transform.position.z.ToFP());
			var rotation = new FQuaternion(
				transform.rotation.x.ToFP(),
				transform.rotation.y.ToFP(),
				transform.rotation.z.ToFP(),
				transform.rotation.w.ToFP());

			serviceLocator.Find<Registry>().Assign(entity, new Body()
			{
				Position = position,
				PrevPosition = position,
				Rotation = rotation,
				PrevRotation = rotation,
				Velocity = _initialLinearVelocity,
				AngularVelocity = _initialAngularVelocity,
				InvMass = invMass,
				InvInertiaTensor = new FVector3(FP.One / inertia, FP.One / inertia, FP.One / inertia)
			});
		}
	}
}
