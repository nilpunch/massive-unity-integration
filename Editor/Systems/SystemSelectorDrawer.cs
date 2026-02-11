#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	[CustomPropertyDrawer(typeof(SystemSelectorAttribute), false)]
	public class SystemSelectorDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var insideArray = SerializedPropertyUtils.IsArrayElement(property);

			var optionalLabel = insideArray ? GUIContent.none : label;
			if (property.managedReferenceValue == null)
			{
				var iconRect = SerializeReferenceGui.DrawPropertySelectorOnly(position, optionalLabel, property, SystemTypeFilter, "System");
				EditorUtils.DrawWarningIcon(iconRect, "This system is null or missing.");
				return;
			}

			SerializeReferenceGui.DrawPropertyWithFoldout(position, optionalLabel, property, SystemTypeFilter, "System");

			bool SystemTypeFilter(Type type)
			{
				return typeof(ISystem).IsAssignableFrom(type);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var height = EditorGUIUtility.singleLineHeight;
			if (!SerializedPropertyUtils.IsTypeMixed(property) && property.managedReferenceValue != null)
			{
				height = EditorGUI.GetPropertyHeight(property, true);
			}
			return height;
		}
	}
}
#endif