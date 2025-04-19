#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	internal static class CleanupStaticHelper
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static void SubscribeCleanup()
		{
			EditorApplication.playModeStateChanged += EditorApplicationOnPlayModeStateChanged;
		}

		private static void EditorApplicationOnPlayModeStateChanged(PlayModeStateChange state)
		{
			if (state == PlayModeStateChange.EnteredEditMode)
			{
				foreach (var world in Worlds.AllWorlds)
				{
					world.Clear();
				}
				EditorApplication.playModeStateChanged -= EditorApplicationOnPlayModeStateChanged;
			}
		}
	}
}
#endif
