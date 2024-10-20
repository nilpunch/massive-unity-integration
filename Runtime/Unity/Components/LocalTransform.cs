using System;
using UnityEngine;

namespace Massive.Unity
{
	[Serializable]
	public struct LocalTransform : IEquatable<LocalTransform>
	{
		public Vector3 Position;
		public Quaternion Rotation;
		public Vector3 Scale;

		public static bool operator ==(LocalTransform a, LocalTransform b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(LocalTransform a, LocalTransform b)
		{
			return !(a == b);
		}

		public bool Equals(LocalTransform other)
		{
			return Position.Equals(other.Position) && Rotation.Equals(other.Rotation) && Scale.Equals(other.Scale);
		}

		public override bool Equals(object obj)
		{
			return obj is LocalTransform other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Position, Rotation, Scale);
		}
	}
}
