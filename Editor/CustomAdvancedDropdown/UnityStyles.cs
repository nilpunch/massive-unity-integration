using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor.UnityInternalBridge
{
	public static class UnityStyles
	{
		public readonly static GUIStyle LargeItem = "DD LargeItemStyle";
		public readonly static GUIStyle LineFaint = new GUIStyle("DD LargeItemStyle");

		static UnityStyles()
		{
			float num = EditorGUIUtility.isProSkin ? 0.5f : 0.25f;
			Color color = new Color(num, num, num, 1f);
			UnityStyles.LineFaint.active.textColor = color;
			UnityStyles.LineFaint.focused.textColor = color;
			UnityStyles.LineFaint.hover.textColor = color;
			UnityStyles.LineFaint.normal.textColor = color;
		}
	}
}
