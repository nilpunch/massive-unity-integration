using System;
using System.Collections.Generic;

namespace Massive.Unity
{
	public class UpdateGroups
	{
		private Dictionary<Type, FastList<Feature>> _groupedFeatures = new Dictionary<Type, FastList<Feature>>();

		public void AddFeature<TGroup>(Feature feature)
		{
			if (!_groupedFeatures.TryGetValue(typeof(TGroup), out var featureList))
			{
				featureList = new FastList<Feature>();
				_groupedFeatures[typeof(TGroup)] = featureList;
			}

			featureList.Add(feature);
		}

		public void Initialize<TGroup>()
		{
			if (_groupedFeatures.TryGetValue(typeof(TGroup), out var featureList))
			{
				foreach (var feature in featureList)
				{
					feature.Initialize();
				}
			}
		}

		public void Update<TGroup>()
		{
			if (_groupedFeatures.TryGetValue(typeof(TGroup), out var featureList))
			{
				foreach (var feature in featureList)
				{
					feature.Update();
				}
			}
		}

		public void Cleanup<TGroup>()
		{
			if (_groupedFeatures.TryGetValue(typeof(TGroup), out var featureList))
			{
				foreach (var feature in featureList)
				{
					feature.Cleanup();
				}
			}
		}
	}
}