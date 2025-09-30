using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor.UnityInternalBridge
{
	public static class UnityStyles
	{
		public static GUIStyle itemStyle = "DD LargeItemStyle";
		public static GUIStyle lineStyleFaint = new GUIStyle("DD LargeItemStyle");

		static UnityStyles()
		{
			float num = EditorGUIUtility.isProSkin ? 0.5f : 0.25f;
			Color color = new Color(num, num, num, 1f);
			UnityStyles.lineStyleFaint.active.textColor = color;
			UnityStyles.lineStyleFaint.focused.textColor = color;
			UnityStyles.lineStyleFaint.hover.textColor = color;
			UnityStyles.lineStyleFaint.normal.textColor = color;
		}
	}
}
