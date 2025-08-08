using System;
using System.Collections.Generic;
using UnityEngine;

namespace Massive.Unity
{
	[DisallowMultipleComponent]
	public class EntityProvider : BaseEntityProvider
	{
		[SerializeReference, ComponentSelector] private List<object> _components = new List<object>();

		private void Start()
		{
			var i = World.Create();

			foreach (var component in _components)
			{
				var sparseSet = World.Sets.GetReflected(component.GetType());
				sparseSet.Add(i);
				if (sparseSet is IDataSet dataSet)
				{
					dataSet.SetRaw(i, component);
				}
			}

			Destroy(gameObject);
		}
	}
}
