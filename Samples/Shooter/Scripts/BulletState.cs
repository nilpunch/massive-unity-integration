using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Massive.Unity.Samples.Cars;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	[Serializable]
	public struct BulletState //, IManaged<BulletState>
	{
		public Vector3 Velocity;

		public float Damage;

		public float Lifetime;

		public bool IsDestroyed
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Lifetime <= 0f;
		}

		// public void CopyTo(ref BulletState other)
		// {
		// 	other = this;
		// }
	}
}
