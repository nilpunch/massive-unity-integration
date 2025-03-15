using Mathematics.Fixed;
using UnityEngine;

namespace Massive.Unity.Samples.Cars
{
	public class CarsTargetingSystem  : UpdateSystem
	{
		[SerializeField] private float _lookaheadRadius = 3f;
		[SerializeField] private float _maxSteeringAngle = 40f;
		[SerializeField] private WaypointsRoute _waypointsRoute;
		
		private Registry _registry;

		public const float HalfPI = Mathf.PI / 2f;
		public const float TwoPI = Mathf.PI * 2f;
		
		public override void Init(ServiceLocator serviceLocator)
		{
			_registry = serviceLocator.Find<Registry>();
		}

		public override void UpdateFrame(FP deltaTime)
		{
			foreach (int entityId in _registry.View().Include<Car>())
			{
				ref var car = ref _registry.Get<Car>(entityId);
				ref var carTransform = ref _registry.Get<LocalTransform>(entityId);

				var lookaheadPosition = _waypointsRoute.GetLookaheadPosition(carTransform.Position.ToXZ(), _lookaheadRadius).FromXZ();

				Vector3 lookaheadPivot = carTransform.Position; // + carTransform.Rotation * Vector3.forward;
				Vector3 lookToAhead = lookaheadPosition - lookaheadPivot;
				float lookaheadHeading = HalfPI - Mathf.Atan2(lookToAhead.z, lookToAhead.x);
				float vehicleHeading = Mathf.Deg2Rad * carTransform.Rotation.eulerAngles.y;
				
				float deltaHeading = (lookaheadHeading - vehicleHeading) % TwoPI;
				if (deltaHeading <= -Mathf.PI)
				{
					deltaHeading += TwoPI;
				}
				else if (deltaHeading > Mathf.PI)
				{
					deltaHeading -= TwoPI;
				}
		
				float targetSteering = Mathf.Clamp(
					Mathf.Atan2(2f * car.WheelBase * Mathf.Sin(Mathf.Clamp(deltaHeading, -HalfPI, HalfPI)) / lookToAhead.magnitude, 1f) * 57.29578f,
					-_maxSteeringAngle * Mathf.Deg2Rad,
					_maxSteeringAngle * Mathf.Deg2Rad);

				car.SteerinAngleRadians = Mathf.MoveTowards(car.SteerinAngleRadians, targetSteering, car.SteerinChangeSpeed * deltaTime.ToFloat());
			}
		}
	}
}