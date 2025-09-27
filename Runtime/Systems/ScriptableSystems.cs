using System;
using Massive.QoL;
using UnityEngine;

namespace Massive.Unity
{
	[CreateAssetMenu(fileName = "Systems", menuName = "Massive")]
	public class ScriptableSystems : ScriptableObject
	{
		[SerializeField] private ScriptableSystem[] _scriptableSystems = Array.Empty<ScriptableSystem>();

		public void Register(SystemsBuilder builder)
		{
			foreach (var scriptableSystem in _scriptableSystems)
			{
				builder.Instance(scriptableSystem);
			}
		}
	}
}
