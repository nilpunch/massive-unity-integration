using System.Runtime.CompilerServices;
using UnityEngine;

namespace Massive.Unity.Samples.Cars
{
	public readonly struct Rotation2D
	{
		public Rotation2D(Angle angle)
		{
			Sin = Mathf.Sin(angle.Radians);
			OneMinusCos = 1f - Mathf.Cos(angle.Radians);
		}

		private Rotation2D(float sin, float oneMinusCos)
		{
			Sin = sin;
			OneMinusCos = oneMinusCos;
		}

		public float Sin { get; }

		public float OneMinusCos { get; }

		public float Cos => 1f - OneMinusCos;

		public Angle CounterclockwiseAngle => Angle.FromRadians(Mathf.Atan2(Sin, Cos));
		
		public Angle ClockwiseAngle => -CounterclockwiseAngle;

		public static Rotation2D Identity => new Rotation2D();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 operator *(Rotation2D rotation2D, Vector2 vector)
		{
			float sin = rotation2D.Sin;
			float cos = rotation2D.Cos;
			return new Vector2(
				vector.x * cos - vector.y * sin,
				vector.x * sin + vector.y * cos
			);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rotation2D operator *(Rotation2D a, Rotation2D b)
		{
			float sinA = a.Sin;
			float cosA = a.Cos;
			float sinB = b.Sin;
			float cosB = b.Cos;

			float cos = cosA * cosB - sinA * sinB;
			float sin = sinA * cosB + cosA * sinB;

			return new Rotation2D(sin, 1f - cos);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rotation2D Inverse(Rotation2D rotation2D)
		{
			return new Rotation2D(-rotation2D.Sin, rotation2D.OneMinusCos);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rotation2D FromToRotation(Vector2 fromDirection, Vector2 toDirection)
		{
			float angleRadians = Mathf.Atan2(toDirection.y, toDirection.x) - Mathf.Atan2(fromDirection.y, fromDirection.x);
			return new Rotation2D(Angle.FromRadians(angleRadians));
		}
	}
}
