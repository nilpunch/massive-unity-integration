#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	public static class EditorUtils
	{
		public static void DrawWarningIcon(Rect rect, string tooltip)
		{
			var icon = EditorGUIUtility.IconContent("console.warnicon");
			icon.tooltip = tooltip;
			GUI.Label(rect, icon);
		}
	}
}
#endif