using Mathematics.Fixed;

namespace Massive.Unity.Samples.Physics
{
	public readonly struct DistanceConstraintSolver : IEntityAction<DistanceConstraint>
	{
		private readonly DataSet<Body> _bodies;
		private readonly FP _invDeltaTimeSqr;

		public DistanceConstraintSolver(DataSet<Body> bodies, FP deltaTime)
		{
			_bodies = bodies;
			_invDeltaTimeSqr = FP.One / (deltaTime * deltaTime);
		}

		public bool Apply(int id, ref DistanceConstraint constraint)
		{
			ref var body = ref _bodies.Get(constraint.Body.Id);
			ref var otherBody = ref _bodies.Get(constraint.OtherBody.Id);

			var worldAttachPoint = body.LocalToWorld(constraint.AttachPoint);
			var otherWorldAttachPoint = otherBody.LocalToWorld(constraint.OtherAttachPoint);

			var difference = otherWorldAttachPoint - worldAttachPoint;
			var distance = FVector3.Length(difference);
			var normal = difference / distance;

			if (distance >= constraint.Distance)
			{
				return true;
			}

			var correction = normal * (distance - constraint.Distance);

			constraint.AppliedForce = BodyUtils.ApplyCorrection(ref body, ref otherBody, constraint.Complience,
				_invDeltaTimeSqr, correction, worldAttachPoint, otherWorldAttachPoint);

			return true;
		}
	}
}
