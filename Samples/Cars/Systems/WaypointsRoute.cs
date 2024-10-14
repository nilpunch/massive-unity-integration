using UnityEngine;

namespace Massive.Unity.Samples.Cars
{
	public class WaypointsRoute : MonoBehaviour
	{
		[SerializeField] private Transform[] _waypoints = new Transform[0];
		[SerializeField] private bool _isLooped = true;

		public Vector2 GetLookaheadPosition(Vector2 carPosition, float lookaheadRadius)
		{
			const int maxSearchDepth = 2;

			var closest = GetClosestPoint(carPosition);

			Vector2 lastPosition = _waypoints[closest.IndexFirst].position.ToXZ();

			Vector2 lookaheadPosition = Vector2.Lerp(_waypoints[closest.IndexFirst].position.ToXZ(), _waypoints[closest.IndexSecond].position.ToXZ(), closest.Fraction);

			int searchDepth = Mathf.Min(maxSearchDepth, _waypoints.Length - 1);
			for (int i = 0; i < searchDepth; i++)
			{
				Vector2 nextPosition = _waypoints[(i + closest.IndexSecond) % _waypoints.Length].position.ToXZ();

				(Vector2 Position, float Fraction)? lineCircleIntersection =
					PurePursuitUtils.LineCircleIntersection(lastPosition, nextPosition, carPosition, lookaheadRadius);

				if (lineCircleIntersection.HasValue)
				{
					lookaheadPosition = Vector2.Lerp(lastPosition, nextPosition, lineCircleIntersection.Value.Fraction);
				}

				lastPosition = nextPosition;
			}

			if (Vector2.Distance(lastPosition, carPosition) < lookaheadRadius)
			{
				return lastPosition;
			}

			return lookaheadPosition;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;

			int segmentsCount = _waypoints.Length - 1;
			if (_isLooped)
			{
				segmentsCount += 1;
			}

			for (int i = 0; i < segmentsCount; i++)
			{
				Vector3 lineStart = _waypoints[i % _waypoints.Length].position;
				Vector3 lineEnd = _waypoints[(i + 1) % _waypoints.Length].position;

				Gizmos.DrawLine(lineStart, lineEnd);
			}
		}

		public (int IndexFirst, int IndexSecond, float Fraction) GetClosestPoint(Vector2 vehiclePosition)
		{
			int closestIndexFirst = -1;
			int closestIndexSecond = -1;
			float closestFraction = 0f;
			float closestDistanceSqr = float.MaxValue;

			var path = _waypoints;

			int segmentsCount = path.Length - 1;
			if (_isLooped)
			{
				segmentsCount += 1;
			}

			for (int i = 0; i < segmentsCount; i++)
			{
				Vector2 lineStart = path[i % path.Length].position.ToXZ();
				Vector2 lineEnd = path[(i + 1) % path.Length].position.ToXZ();

				Vector2 lineDirection = lineEnd - lineStart;
				float lineLength = lineDirection.magnitude;

				float distanceSqr;
				float fraction;

				if (lineLength <= 0.00001f)
				{
					distanceSqr = Vector2.SqrMagnitude(lineStart - vehiclePosition);
					fraction = 0f;
				}
				else
				{
					Vector2 lineUnitDirection = lineDirection / lineLength;

					Vector2 fromStartToPoint = vehiclePosition - lineStart;

					float dotProduct = Vector2.Dot(fromStartToPoint, lineUnitDirection);
					dotProduct = Mathf.Clamp(dotProduct, 0f, lineLength);

					Vector2 closest = lineStart + lineUnitDirection * dotProduct;

					distanceSqr = Vector2.SqrMagnitude(closest - vehiclePosition);
					fraction = dotProduct / lineLength;
				}

				// Update the closest point if a smaller distance is found
				if (distanceSqr < closestDistanceSqr)
				{
					closestDistanceSqr = distanceSqr;
					closestIndexFirst = i % path.Length;
					closestIndexSecond = (i + 1) % path.Length;
					closestFraction = fraction;
				}
			}

			return (closestIndexFirst, closestIndexSecond, closestFraction);
		}
	}
}