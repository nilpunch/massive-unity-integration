using System;
using System.Reflection;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Massive.Unity.Editor.UnityInternalBridge
{
	public abstract class CustomAdvancedDropdownItem : AdvancedDropdownItem
	{
		private static readonly Action<AdvancedDropdownItem, string> SetName;

		static CustomAdvancedDropdownItem()
		{
			var fieldInfo = typeof(AdvancedDropdownItem).GetField("m_Name", BindingFlags.NonPublic | BindingFlags.Instance);
			if (fieldInfo != null)
			{
				SetName = (item, name) => fieldInfo.SetValue(item, name);
			}
		}

		protected CustomAdvancedDropdownItem(string contentName, string searchName = null) : base(contentName)
		{
			SetName?.Invoke(this, searchName ?? contentName);
		}

		public virtual void DrawItem(
			Rect rect,
			GUIStyle mainStyle,
			bool isHover,
			bool isActive,
			bool on,
			bool hasKeyboardFocus)
		{
			mainStyle.Draw(rect, content, isHover, isActive, on, hasKeyboardFocus);
		}
	}
}
