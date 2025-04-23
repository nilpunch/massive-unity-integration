using System;
using UnityEngine;

namespace Massive.Unity
{
	[Serializable]
	public class ScriptableFeature
	{
		[SerializeField] private ScriptableSystem[] _scriptableSystems;

		public Feature CreateFeature(World world)
		{
			var feature = new Feature(world);

			foreach (var scriptableSystem in _scriptableSystems)
			{
				feature.AddSystem(scriptableSystem);
			}

			return feature;
		}
	}
}
