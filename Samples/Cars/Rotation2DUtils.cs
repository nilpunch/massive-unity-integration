using System.Runtime.CompilerServices;
using UnityEngine;

namespace Massive.Unity.Samples.Cars
{
	public static class Rotation2DUtils
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Rotation2D Inverse(this Rotation2D rotation2D)
		{
			return Rotation2D.Inverse(rotation2D);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 UpAxis(this Rotation2D rotation2D)
		{
			return new Vector2(-rotation2D.Sin, rotation2D.Cos);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 RightAxis(this Rotation2D rotation2D)
		{
			return new Vector2(rotation2D.Cos, rotation2D.Sin);
		}
	}
}
