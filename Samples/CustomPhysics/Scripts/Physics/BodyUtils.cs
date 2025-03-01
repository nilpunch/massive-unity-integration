using System.Runtime.CompilerServices;
using Mathematics.Fixed;

namespace Massive.Unity.Samples.Physics
{
	public static class BodyUtils
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FVector3 LocalToWorld(this in Body body, FVector3 point)
		{
			return body.Position + body.Rotation * point;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FVector3 WorldToLocal(this in Body body, FVector3 point)
		{
			return FQuaternion.Inverse(body.Rotation) * (point - body.Position);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FVector3 LocalVelocityAtPosition(this in Body body, FVector3 point)
		{
			return body.Velocity + FVector3.Cross(body.AngularVelocity, point);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FP EffectiveInverseMass(this in Body body, FVector3 constraintDirection, FVector3 applicationPoint)
		{
			if (body.InvMass == 0)
			{
				return FP.Zero;
			}

			var leverArm = applicationPoint - body.Position;
			var angularInfluence = FVector3.Cross(leverArm, constraintDirection);
			angularInfluence = FQuaternion.Inverse(body.Rotation) * angularInfluence;

			var effectiveInvMass =
				angularInfluence.X * angularInfluence.X * body.InvInertiaTensor.X +
				angularInfluence.Y * angularInfluence.Y * body.InvInertiaTensor.Y +
				angularInfluence.Z * angularInfluence.Z * body.InvInertiaTensor.Z;

			effectiveInvMass += body.InvMass;

			return effectiveInvMass;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void ApplyCorrection(this ref Body body, FVector3 impulse, FVector3 applicationPoint)
		{
			if (body.InvMass == 0)
			{
				return;
			}

			body.Position += impulse * body.InvMass;

			var leverArm = applicationPoint - body.Position;
			var angularImpulse = FVector3.Cross(leverArm, impulse);
			angularImpulse = FQuaternion.Inverse(body.Rotation) * angularImpulse;
			angularImpulse *= body.InvInertiaTensor;
			angularImpulse = body.Rotation * angularImpulse;

			var deltaRotation = new FQuaternion(
				angularImpulse.X,
				angularImpulse.Y,
				angularImpulse.Z,
				FP.Zero);

			body.Rotation = FQuaternion.Normalize(body.Rotation + FP.Half * (deltaRotation * body.Rotation));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FP ApplyCorrection(
			ref Body body, ref Body otherBody, FP compliance, FP invDeltaTimeSqr,
			FVector3 correction, FVector3 applicationPoint, FVector3 otherApplicationPoint)
		{
			var correctionMagnitude = FVector3.Length(correction);
			if (correctionMagnitude == FP.Zero)
			{
				return FP.Zero;
			}

			var constraintDirection = correction / correctionMagnitude;

			// Compute the total generalized inverse mass for both bodies.
			var totalInvMass = body.EffectiveInverseMass(constraintDirection, applicationPoint) +
				otherBody.EffectiveInverseMass(constraintDirection, otherApplicationPoint);

			if (totalInvMass == FP.Zero)
			{
				return FP.Zero;
			}

			// XPBD.
			var alpha = compliance * invDeltaTimeSqr;
			var lambda = -correctionMagnitude / (totalInvMass + alpha);
			var impulse = constraintDirection * -lambda;

			// Apply corrections to both bodies.
			body.ApplyCorrection(impulse, applicationPoint);
			otherBody.ApplyCorrection(-impulse, otherApplicationPoint);

			return lambda * invDeltaTimeSqr;
		}
	}
}
