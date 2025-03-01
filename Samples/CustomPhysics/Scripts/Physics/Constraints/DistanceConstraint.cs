using Mathematics.Fixed;

namespace Massive.Unity.Samples.Physics
{
	public struct DistanceConstraint
	{
		public Entity Body;
		public Entity OtherBody;

		public FVector3 AttachPoint;
		public FVector3 OtherAttachPoint;

		public FP Distance;
		public FP Complience;

		public FP AppliedForce;
	}
}
