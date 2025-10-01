using System;

namespace Massive.Unity.Samples.Farm
{
	[Serializable, Component]
	public struct Mature
	{
		public Seed _seed;
		public float FruitElapsedTime;
		public int Fruits;
	}
}
