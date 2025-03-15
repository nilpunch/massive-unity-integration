using System.Runtime.CompilerServices;
using Mathematics.Fixed;
using UnityEngine;

namespace Massive.Unity.Samples.Physics
{
	public static class FMathConversions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FVector3 ToFVector3(this Vector3 value)
		{
			return new FVector3(
				value.x.ToFP(),
				value.y.ToFP(),
				value.z.ToFP());
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ToVector3(this FVector3 value)
		{
			return new Vector3(
				value.X.ToFloat(),
				value.Y.ToFloat(),
				value.Z.ToFloat());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static FQuaternion ToFQuaternion(this Quaternion value)
		{
			return new FQuaternion(
				value.x.ToFP(),
				value.y.ToFP(),
				value.z.ToFP(),
				value.w.ToFP());
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Quaternion ToQuaternion(this FQuaternion value)
		{
			return new Quaternion(
				value.X.ToFloat(),
				value.Y.ToFloat(),
				value.Z.ToFloat(),
				value.W.ToFloat());
		}
	}
}
