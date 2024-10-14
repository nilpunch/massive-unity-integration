using System;

namespace Massive.Unity.Samples.Cars
{
	[Serializable]
	public struct Car
	{
		public float ForwardVelocity;
		public float SteerinAngleRadians;
		public float SteerinChangeSpeed;
		public float RotationPivotOffset;
		public float WheelBase;
	}
}
