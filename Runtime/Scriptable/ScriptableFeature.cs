using System;
using UnityEngine;

namespace Massive.Unity
{
	[Serializable]
	public class ScriptableFeature
	{
		[SerializeField] private ScriptableSystem[] _scriptableSystems;

		public void PopulateFeature(Feature feature)
		{
			foreach (var scriptableSystem in _scriptableSystems)
			{
				feature.AddSystem(scriptableSystem);
			}
		}
	}
}
