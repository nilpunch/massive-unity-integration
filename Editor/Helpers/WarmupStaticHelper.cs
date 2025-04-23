#if UNITY_EDITOR
using UnityEditor;

namespace Massive.Unity.Editor
{
	[InitializeOnLoad]
	internal static class WarmupStaticHelper
	{
		static WarmupStaticHelper()
		{
			StaticWorlds.WarmupAll();
			Components.Warmup();
		}
	}
}
#endif
