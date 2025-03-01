using Mathematics.Fixed;

namespace Massive.Unity.Samples.Physics
{
	public struct Body
	{
		public FVector3 Position;
		public FVector3 Velocity;
		public FVector3 PrevPosition;

		public FQuaternion Rotation;
		public FVector3 AngularVelocity;
		public FQuaternion PrevRotation;

		public FP InvMass;
		public FVector3 InvInertiaTensor;
	}
}
