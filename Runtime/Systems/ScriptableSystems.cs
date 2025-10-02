using System;
using System.Collections.Generic;
using Massive.QoL;
using UnityEngine;

namespace Massive.Unity
{
	[CreateAssetMenu(fileName = "Systems", menuName = "Massive/Systems")]
	public class ScriptableSystems : ScriptableObject
	{
		[SerializeReference, SystemSelector] private List<ISystem> _systems = new List<ISystem>();

		public void Register(SystemsBuilder builder)
		{
			foreach (var system in _systems)
			{
				builder.Instance(system);
			}
		}
	}
}
