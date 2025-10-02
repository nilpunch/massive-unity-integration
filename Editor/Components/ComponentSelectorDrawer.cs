#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	[CustomPropertyDrawer(typeof(ComponentSelectorAttribute), false)]
	public class ComponentSelectorDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var insideArray = SerializedPropertyUtils.IsArrayElement(property);

			SerializedPropertyUtils.CollectPresentTypes(property);

			var optionalLabel = insideArray ? GUIContent.none : label;
			if (property.managedReferenceValue == null)
			{
				var iconRect = SerializeReferenceGui.DrawPropertySelectorOnly(position, optionalLabel, property, ComponentTypeFilter, "Component");
				EditorUtils.DrawWarningIcon(iconRect, "This component is null or missing.");
				return;
			}

			var isTypeMixed = SerializedPropertyUtils.IsTypeMixed(property);

			if (!isTypeMixed && SerializedPropertyUtils.HasDuplicateType(property))
			{
				var iconRect = SerializeReferenceGui.DrawPropertySelectorOnly(position, optionalLabel, property, ComponentTypeFilter, "Component");
				EditorUtils.DrawWarningIcon(iconRect, "This component type is already added. Only one is allowed per entity.");
				return;
			}

			SerializeReferenceGui.DrawPropertyWithFoldout(position, optionalLabel, property, ComponentTypeFilter, "Component");

			bool ComponentTypeFilter(Type type)
			{
				return type.IsDefined(typeof(ComponentAttribute), false) && !SerializedPropertyUtils.PresentTypesBuffer.Contains(type);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var height = EditorGUIUtility.singleLineHeight;
			if (!SerializedPropertyUtils.IsTypeMixed(property) && !SerializedPropertyUtils.HasDuplicateType(property) && property.managedReferenceValue != null)
			{
				height = EditorGUI.GetPropertyHeight(property, true);
			}
			return height;
		}
	}
}
#endif
