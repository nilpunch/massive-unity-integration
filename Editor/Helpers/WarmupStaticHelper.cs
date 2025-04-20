#if UNITY_EDITOR
using UnityEditor;

namespace Massive.Unity.Editor
{
	[InitializeOnLoad]
	internal static class WarmupStaticHelper
	{
		static WarmupStaticHelper()
		{
			Worlds.WarmupAll();
			Components.Warmup();
		}
	}
}
#endif
