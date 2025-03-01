using Mathematics.Fixed;

namespace Massive.Unity.Samples.Physics
{
	public class StaticPhysicsBodyProvider : ComponentProvider
	{
		public override void ApplyToEntity(ServiceLocator serviceLocator, Entity entity)
		{
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
				InvMass = FP.Zero,
				InvInertiaTensor = FVector3.Zero
			});
		}
	}
}
