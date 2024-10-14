using System.Runtime.CompilerServices;
using UnityEngine;

namespace Massive.Unity.Samples.Cars
{
	public static class Vector2Utils
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 RotateAround(this Vector2 vector, Vector2 rotationOrigin, Rotation2D rotation2D)
		{
			return rotation2D * (vector - rotationOrigin) + rotationOrigin;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 With(this Vector2 vector, float? x = null, float? y = null)
		{
			return new Vector2
			{
				x = x ?? vector.x,
				y = y ?? vector.y
			};
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 FromXZ(this Vector2 vector, float y = 0) => new Vector3(vector.x, y, vector.y);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 FromXY(this Vector2 vector, float z = 0) => new Vector3(vector.x, vector.y, z);
	}
}
