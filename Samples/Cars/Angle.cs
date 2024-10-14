using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Massive.Unity.Samples.Cars
{
	/// <summary>
	/// Representation of an angle.
	/// </summary>
	public readonly struct Angle: IEquatable<Angle>
	{
		private const float PIRad = Mathf.PI;
		private const float TwoPIRad = PIRad * 2f;
		private const float HalfPIRad = PIRad * 0.5f;
		
		public enum RotationAxis
		{
			X,
			Y,
			Z
		}

		private Angle(float radians)
		{
			Radians = radians;
		}

		public float Radians { get; }

		public float Degrees => Radians * Mathf.Rad2Deg;
		
		public Rotation2D Counterclockwise => new Rotation2D(this);
		
		public Rotation2D Clockwise => new Rotation2D(new Angle(-Radians));

		public static Angle Zero => new Angle(0f);
		
		public static Angle TwoPI => new Angle(TwoPIRad);
		
		public static Angle HalfPI => new Angle(HalfPIRad);
		
		public static Angle PI => new Angle(PIRad);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle FromDegrees(float degrees)
		{
			return new Angle(degrees * Mathf.Deg2Rad);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle FromRadians(float radians)
		{
			return new Angle(radians);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle FromQuaternionY(Quaternion quaternion)
		{
			return FromQuaternion(quaternion, RotationAxis.Y);
		}

		public static Angle Normalize360(Angle angle)
		{
			float angleRad = angle.Radians % TwoPIRad;

			if (angleRad < 0)
			{
				angleRad += TwoPIRad;
			}
			else if (angleRad >= TwoPIRad)
			{
				angleRad -= TwoPIRad;
			}

			return new Angle(angleRad);
		}
		
		public static Angle Normalize180(Angle angle)
		{
			float angleRad = angle.Radians % TwoPIRad;

			if (angleRad <= -PIRad)
			{
				angleRad += TwoPIRad;
			}
			else if (angleRad > PIRad)
			{
				angleRad -= TwoPIRad;
			}

			return new Angle(angleRad);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle Lerp360(Angle from, Angle to, float factor)
		{
			Angle difference = Normalize180(to - from);

			return Normalize360(from + difference * Mathf.Clamp01(factor));
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle Lerp180(Angle from, Angle to, float factor)
		{
			Angle difference = Normalize180(to - from);

			return Normalize180(from + difference * Mathf.Clamp01(factor));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle Abs(Angle angle)
		{
			return new Angle(Mathf.Abs(angle.Radians));
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle Delta(Angle from, Angle to)
		{
			return Normalize180(to - from);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle UnsignedDelta(Angle from, Angle to)
		{
			return Abs(Delta(from, to));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle MoveTowards(Angle from, Angle to, Angle maxDelta)
		{
			Angle delta = Angle.Delta(from, to);
			
			if (-maxDelta < delta && delta < maxDelta)
				return to;
			
			return Angle.FromRadians(Mathf.MoveTowards(from.Radians, (from + delta).Radians, maxDelta.Radians));
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle Max(Angle a, Angle b)
		{
			return Angle.FromRadians(Mathf.Max(a.Radians, b.Radians));
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle Min(Angle a, Angle b)
		{
			return Angle.FromRadians(Mathf.Min(a.Radians, b.Radians));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle operator *(Angle angle, float value)
		{
			return new Angle(angle.Radians * value);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle operator *(float value, Angle angle)
		{
			return angle * value;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle operator *(Angle angle, int value)
		{
			return new Angle(angle.Radians * value);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle operator *(int value, Angle angle)
		{
			return angle * value;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle operator /(Angle angle, float value)
		{
			return new Angle(angle.Radians / value);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle operator /(Angle angle, int value)
		{
			return new Angle(angle.Radians / value);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle operator +(Angle a, Angle b)
		{
			return new Angle(a.Radians + b.Radians);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle operator -(Angle a)
		{
			return new Angle(-a.Radians);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Angle operator -(Angle a, Angle b)
		{
			return -b + a;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Angle a, Angle b)
		{
			return Mathf.Approximately(a.Radians, b.Radians);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Angle a, Angle b)
		{
			return !(a == b);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >=(Angle a, Angle b)
		{
			return a.Radians >= b.Radians;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <=(Angle a, Angle b)
		{
			return a.Radians <= b.Radians;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator >(Angle a, Angle b)
		{
			return a.Radians > b.Radians;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator <(Angle a, Angle b)
		{
			return a.Radians < b.Radians;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object obj)
		{
			return obj is Angle other && Equals(other);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(Angle other) => Radians.Equals(other.Radians);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode() => Radians.GetHashCode();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString() => $"{Degrees}°";

		public static Angle FromQuaternion(Quaternion quaternion, RotationAxis rotationAxis = RotationAxis.Y)
		{
			float test = quaternion.x * quaternion.y + quaternion.z * quaternion.w;
			if (test > 0.4999f) // Singularity at north pole
			{
				switch (rotationAxis)
				{
					case RotationAxis.Y: return new Angle(2 * Mathf.Atan2(quaternion.x, quaternion.w));
					case RotationAxis.Z: return new Angle(PIRad / 2);
					case RotationAxis.X: return new Angle(0);
					default: throw new Exception("This cannot happened! Check input axis.");
				}
			}
			if (test < -0.4999f) // Singularity at south pole
			{
				switch (rotationAxis)
				{
					case RotationAxis.Y: return new Angle(-2 * Mathf.Atan2(quaternion.x, quaternion.w));
					case RotationAxis.Z: return new Angle(-PIRad / 2);
					case RotationAxis.X: return new Angle(0);
					default: throw new Exception("This cannot happened! Check input axis.");
				}
			}
			switch (rotationAxis)
			{
				case RotationAxis.Y: return new Angle(Mathf.Atan2(2 * quaternion.y * quaternion.w - 2 * quaternion.x * quaternion.z, 1 - 2 * (quaternion.y * quaternion.y) - 2 * (quaternion.z * quaternion.z)));
				case RotationAxis.Z: return new Angle(Mathf.Asin(2 * test));
				case RotationAxis.X: return new Angle(Mathf.Atan2(2 * quaternion.x * quaternion.w - 2 * quaternion.y * quaternion.z, 1 - 2 * (quaternion.x * quaternion.x) - 2 * (quaternion.z * quaternion.z)));
				default: throw new Exception("This cannot happened! Check input axis.");
			}
		}
	}
}
