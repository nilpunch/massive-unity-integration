using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Massive.Unity.Editor.UnityInternalBridge
{
	public abstract class CustomAdvancedDropdown : AdvancedDropdown
	{
		protected CustomAdvancedDropdown(AdvancedDropdownState state) : base(state)
		{
			m_DataSource = new CallbackDataSource(BuildRoot);
			m_Gui = new CustomAdvancedDropdownGUI(m_DataSource);
		}

		public new void Show(Rect rect)
		{
			base.Show(rect);
			m_WindowInstance.selectionCanceled += OnSelectionCanceled;
		}

		private void OnSelectionCanceled()
		{
			m_WindowInstance.selectionCanceled -= OnSelectionCanceled;
			ItemSelected(null);
		}
	}
}
