using UnityEngine;

namespace Massive.Unity
{
	internal static class StaticWorldsWarmupHelper
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
		private static void WarmupStaticWorlds()
		{
			StaticWorlds.WarmupAll();
		}
	}
}
