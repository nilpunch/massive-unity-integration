using Massive.QoL;
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

	public class PlantFruitingSystem : QoL.System, IUpdate, IDrawGizmos
	{
		public void Update()
		{
			var plants = World.DataSet<Plant>();
			var matures = World.DataSet<Mature>();

			foreach (var i in World.Include<Plant, Mature>())
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

		public void OnDrawGizmos()
		{
			
		}
	}
}
