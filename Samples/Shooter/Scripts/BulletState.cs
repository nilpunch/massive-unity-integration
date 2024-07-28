using System;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	[Serializable]
	public struct BulletState
	{
		public Vector3 Velocity;

		public float Damage;

		public float Lifetime;

		public bool IsDestroyed => Lifetime <= 0f;
	}
}
