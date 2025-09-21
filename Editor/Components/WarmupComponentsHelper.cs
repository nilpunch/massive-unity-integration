#if UNITY_EDITOR
using UnityEditor;

namespace Massive.Unity.Editor
{
	[InitializeOnLoad]
	internal static class WarmupComponentsHelper
	{
		static WarmupComponentsHelper()
		{
			Components.Warmup();
		}
	}
}
#endif
