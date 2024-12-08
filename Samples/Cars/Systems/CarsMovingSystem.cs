// using UnityEngine;
//
// namespace Massive.Unity.Samples.Cars
// {
// 	public class CarsMovingSystem  : UpdateSystem
// 	{
// 		private Registry _registry;
//
// 		public override void Init(Registry registry)
// 		{
// 			_registry = registry;
// 		}
// 		
// 		public override void UpdateFrame(float deltaTime)
// 		{
// 			_registry.View().ForEachExtra(deltaTime,
// 				static (ref Car car, ref LocalTransform carTransform, float deltaTime) =>
// 				{
// 					Vector3 forward = carTransform.Rotation * Vector3.forward;
//
// 					Vector2 position = carTransform.Position.ToXZ();
// 					Angle heading = Rotation2D.FromToRotation(Vector2.up, forward.ToXZ()).ClockwiseAngle;
// 					Vector2 pivot = position + heading.Clockwise * Vector2.up * car.RotationPivotOffset;
//
// 					float movement = car.ForwardVelocity * deltaTime;
// 					float turningAngle = movement / car.WheelBase * Mathf.Sin(car.SteerinAngleRadians);
// 					float turningAngleForRealisticMovement = movement / car.WheelBase * Mathf.Tan(car.SteerinAngleRadians);
//
// 					var newPivotPosition = CalculateNewPosition(heading.Radians, turningAngleForRealisticMovement, movement, pivot);
// 					var newHeading = heading + Angle.FromRadians(turningAngle);
// 					var newPosition = newPivotPosition - newHeading.Clockwise * Vector2.up * car.RotationPivotOffset;
//
// 					Quaternion newRotation = Quaternion.AngleAxis(newHeading.Degrees, Vector3.up);
//
// 					carTransform.Position = newPosition.FromXZ(carTransform.Position.y);
// 					carTransform.Rotation = newRotation;
// 				});
// 		}
//
// 		public static Vector2 CalculateNewPosition(
// 			float headingRad,
// 			float steeringAngle,
// 			float distance,
// 			Vector2 pivotWheelPos)
// 		{
// 			Vector2 newPivotWheelPos = Vector2.zero;
//
// 			if (Mathf.Abs(steeringAngle) < 0.00001f)
// 			{
// 				newPivotWheelPos.x = pivotWheelPos.x + distance * Mathf.Sin(headingRad);
// 				newPivotWheelPos.y = pivotWheelPos.y + distance * Mathf.Cos(headingRad);
// 			}
// 			else
// 			{
// 				float radius = distance / steeringAngle;
// 				float cx = pivotWheelPos.x + Mathf.Cos(headingRad) * radius;
// 				float cz = pivotWheelPos.y - Mathf.Sin(headingRad) * radius;
//
// 				newPivotWheelPos.x = cx - Mathf.Cos(headingRad + steeringAngle) * radius;
// 				newPivotWheelPos.y = cz + Mathf.Sin(headingRad + steeringAngle) * radius;
// 			}
//
// 			return newPivotWheelPos;
// 		}
// 		
// 		private void OnGUI()
// 		{
// 			float fontScaling = Screen.height / (float)1080;
//
// 			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
//
// 			GUILayout.BeginVertical();
//
// 			GUILayout.FlexibleSpace();
//
// 			GUILayout.TextField($"{_registry.Set<Car>().Count} Cars",
// 				new GUIStyle() { fontSize = Mathf.RoundToInt(70 * fontScaling), normal = new GUIStyleState() { textColor = Color.white } });
//
// 			GUILayout.EndVertical();
//
// 			GUILayout.EndArea();
// 		}
// 	}
// }