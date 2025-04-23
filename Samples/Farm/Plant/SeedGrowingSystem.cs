using UnityEngine;

namespace Massive.Unity.Samples.Farm
{
	public class SeedGrowingSystem : System, IUpdate
	{
		public void Update()
		{
			var plants = World.DataSet<Plant>();
			var seeds = World.DataSet<Seed>();
			var matures = World.DataSet<Mature>();
			
			foreach (var i in View.Include<Plant, Seed>())
			{
				ref var plant = ref plants.Get(i);
				ref var seed = ref seeds.Get(i);

				seed.ElapsedTime += Time.deltaTime;

				if (seed.ElapsedTime > plant.GrowTime)
				{
					seeds.Remove(i);
					matures.Add(i);
				}
			}
		}
	}
}
