using System;
using System.Collections.Generic;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	[Serializable]
	public struct WeaponState
	{
		[Range(0f, 1f)]
		public float Cooldown;
		public SomeStatus _someStatus;
	}

	public class RegistryPrewarm
	{
		private List<ISetSelector> _selectors = new List<ISetSelector>();
		
		public void Register<T>()
		{
			
		}
		
		public void RegisterGroup<TInclude, TExclude, TOwned>()
		{
			
		}

		public void Prewarm(Registry registry)
		{
			foreach (var selector in _selectors)
			{
				selector.Select(registry.SetRegistry);
			}
		}
	}
}
