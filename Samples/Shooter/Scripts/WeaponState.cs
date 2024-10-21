using System;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	[Serializable]
	public struct WeaponState
	{
		[Range(0f, 1f)]
		public float Cooldown;
		public WeaponStatus WeaponStatus;
	}
}
