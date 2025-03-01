using Mathematics.Fixed;

namespace Massive.Unity.Samples.Physics
{
	public readonly struct Integration : IEntityAction<Body>
	{
		private readonly FP _deltaTime;
		private readonly FVector3 _gravityVelocityChange;
		private readonly FP _halfDeltaTime;

		public Integration(FP deltaTime, FVector3 gravity)
		{
			_deltaTime = deltaTime;
			_gravityVelocityChange = gravity * _deltaTime;
			_halfDeltaTime = FP.Half * _deltaTime;
		}

		public bool Apply(int id, ref Body body)
		{
			if (body.InvMass == 0)
			{
				return true;
			}

			// Linear motion.
			body.PrevPosition = body.Position;
			body.Velocity += _gravityVelocityChange;
			body.Position += body.Velocity * _deltaTime;

			// Angular motion.
			body.PrevRotation = body.Rotation;
			var angularVelocity = body.AngularVelocity;
			var velocityRotation = new FQuaternion(
				angularVelocity.X,
				angularVelocity.Y,
				angularVelocity.Y,
				FP.Zero);
			body.Rotation = FQuaternion.Normalize(body.Rotation + _halfDeltaTime * (velocityRotation * body.Rotation));

			return true;
		}
	}
}
