using Mathematics.Fixed;

namespace Massive.Unity.Samples.Physics
{
	public readonly struct VelocityUpdate : IEntityAction<Body>
	{
		private readonly FP _invDeltaTime;
		private readonly FP _twoDivDeltaTime;

		public VelocityUpdate(FP deltaTime)
		{
			_invDeltaTime = FP.One / deltaTime;
			_twoDivDeltaTime = FP.Two / deltaTime;
		}

		public bool Apply(int id, ref Body body)
		{
			// Linear velocity update.
			body.Velocity = (body.Position - body.PrevPosition) * _invDeltaTime;

			// Angular velocity update.
			var deltaRotation = body.Rotation * FQuaternion.Inverse(body.PrevRotation);
			body.AngularVelocity = new FVector3(
				deltaRotation.X * _twoDivDeltaTime,
				deltaRotation.Y * _twoDivDeltaTime,
				deltaRotation.Z * _twoDivDeltaTime);
			if (deltaRotation.W < 0)
			{
				body.AngularVelocity = -body.AngularVelocity;
			}

			return true;
		}
	}
}
