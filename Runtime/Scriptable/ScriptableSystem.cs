using Massive.QoL;
using UnityEngine;

namespace Massive.Unity
{
	public abstract class ScriptableSystem : ScriptableObject, ISystem
	{
		public World World { get; set; }
	}
}
