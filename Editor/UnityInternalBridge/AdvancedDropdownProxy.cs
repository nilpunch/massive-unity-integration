using UnityEditor.IMGUI.Controls;

namespace Massive.Unity.Editor.UnityInternalBridge
{
	public class AdvancedDropdownProxy
	{
		public static void SetShowHeader(AdvancedDropdown dropdown, bool showHeader)
		{
			dropdown.m_WindowInstance.showHeader = showHeader;
		}
	}
}
