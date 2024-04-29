using System;
using Massive;
using UnityEngine;

namespace Massive.Unity
{
	public struct Hierarchy
	{
		public Entity Parent;

		public Entity Child;

		public Entity First;
		public Entity Next;
		public Entity Prev;
	}

	public struct Transform : IEquatable<Transform>
	{
		public Vector3 LocalPosition;
		public Quaternion LocalRotation;
		public Vector3 LocalScale;

		public static bool operator ==(Transform a, Transform b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(Transform a, Transform b)
		{
			return !(a == b);
		}

		public bool Equals(Transform other)
		{
			return LocalPosition.Equals(other.LocalPosition) && LocalRotation.Equals(other.LocalRotation) && LocalScale.Equals(other.LocalScale);
		}

		public override bool Equals(object obj)
		{
			return obj is Transform other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(LocalPosition, LocalRotation, LocalScale);
		}
	}
}
