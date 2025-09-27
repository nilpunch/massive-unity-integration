using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Massive.Unity.Editor.UnityInternalBridge
{
	internal class CustomAdvancedDropdownGUI : AdvancedDropdownGUI
	{
		internal override GUIStyle lineStyle => UnityStyles.itemStyle;

		public CustomAdvancedDropdownGUI(AdvancedDropdownDataSource dataSource) : base(dataSource)
		{
		}

		internal override void DrawItemContent(
			AdvancedDropdownItem item,
			Rect rect,
			GUIContent content,
			bool isHover,
			bool isActive,
			bool on,
			bool hasKeyboardFocus)
		{
			if (item is CustomAdvancedDropdownItem customItem)
			{
				customItem.DrawItem(rect, lineStyle, isHover, isActive, on, hasKeyboardFocus);
			}
			else
			{
				this.lineStyle.Draw(rect, content, isHover, isActive, on, hasKeyboardFocus);
			}
		}
	}
}
