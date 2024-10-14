using UnityEngine;

namespace Massive.Unity.Samples.Cars
{
	public static class PurePursuitUtils
	{
		public static (Vector2 Position, float Fraction)? LineCircleIntersection(Vector2 begin, Vector2 end, Vector2 circleCenter, float circleRadius)
		{
			Vector2 lineDirection = end - begin;
			Vector2 lookFromCircleToStart = begin - circleCenter;

			var a = Vector2.Dot(lineDirection, lineDirection);
			var b = 2 * Vector2.Dot(lookFromCircleToStart, lineDirection);
			var c = Vector2.Dot(lookFromCircleToStart, lookFromCircleToStart) - circleRadius * circleRadius;

			var discriminant = b * b - 4 * a * c;

			if (discriminant < 0f)
			{
				return null;
			}
			else
			{
				discriminant = Mathf.Sqrt(discriminant);

				var t1 = (-b - discriminant) / (2 * a);
				var t2 = (-b + discriminant) / (2 * a);

				if (t1 >= 0 && t1 <= 1 && t2 >= 0 && t2 <= 1)
				{
					if (t1 > t2)
					{
						return (begin + t1 * lineDirection, t1);
					}
					else
					{
						return (begin + t2 * lineDirection, t2);
					}
				}
				
				if (t1 >= 0 && t1 <= 1)
				{
					return (begin + t1 * lineDirection, t1);
				}
				else if (t2 >= 0 && t2 <= 1)
				{
					return (begin + t2 * lineDirection, t2);
				}

				return null;
			}
		}
	}
}