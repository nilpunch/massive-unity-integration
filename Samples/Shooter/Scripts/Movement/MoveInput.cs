using System;
using System.Runtime.CompilerServices;
using Massive.Netcode;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	[Serializable]
	public struct MoveInput : IFadeOutInput<MoveInput>
	{
		public Vector3 Direction;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MoveInput FadeOut(int ticksPassed, in FadeOutConfig config)
		{
			float fadeOutPercent = Math.Clamp((ticksPassed - config.StartDecayTick) / (float)config.DecayDurationTicks, 0f, 1f);
			float modifier = 1f - fadeOutPercent;
			return new MoveInput() { Direction = Direction * modifier };
		}
	}
}
