#if UNITY_EDITOR
using UnityEditor;

namespace Massive.Unity.Editor
{
	[InitializeOnLoad]
	internal static class WarmupEditorStaticHelper
	{
		static WarmupEditorStaticHelper()
		{
			Worlds.WarmupAll();
		}
	}
}
#endif
