using UnityEngine;

namespace Massive.Unity
{
	internal static class WarmupStaticHelper
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		private static void WarmupStaticWorlds()
		{
			Worlds.WarmupAll();
		}
	}
}
