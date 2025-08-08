using UnityEngine;

namespace Massive.Unity.Samples.Farm
{
	public class PlantGrowthFeatureFactory : FeatureFactory
	{
		public PlantGrowthFeatureFactory()
		{
			AddNew<SeedGrowingSystem>();
			AddNew<PlantFruitingSystem>();
		}
	}

	public class PlantFruitingSystem : System, IUpdate
	{
		public void Update()
		{
			var plants = World.DataSet<Plant>();
			var matures = World.DataSet<Mature>();

			foreach (var i in View.Include<Plant, Mature>())
			{
				ref var plant = ref plants.Get(i);
				ref var mature = ref matures.Get(i);

				mature.FruitElapsedTime += Time.deltaTime;

				if (mature.FruitElapsedTime >= plant.TimePerFruit)
				{
					mature.FruitElapsedTime = 0;
					mature.Fruits += 1;
				}
			}
		}
	}
}
