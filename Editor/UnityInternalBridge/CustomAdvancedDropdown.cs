using UnityEditor.IMGUI.Controls;

namespace Massive.Unity.Editor.UnityInternalBridge
{
	public abstract class CustomAdvancedDropdown : AdvancedDropdown
	{
		protected CustomAdvancedDropdown(AdvancedDropdownState state) : base(state)
		{
			m_DataSource = new CallbackDataSource(BuildRoot);
			m_Gui = new CustomAdvancedDropdownGUI(m_DataSource);
		}
	}
}
