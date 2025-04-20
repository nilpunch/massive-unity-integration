#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	[CustomPropertyDrawer(typeof(ComponentPeekerAttribute), false)]
	public class ComponentSelectorDrawer : PropertyDrawer
	{
		private const float PopupLeftPadding = 8f;
		private const int FoldoutOffset = 5;
		private const int WarningIconOffset = -9;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var lineHeight = EditorGUIUtility.singleLineHeight;
			var y = position.y;

			var isTypeMixed = IsTypeMixed(property);

			// Draw popup
			var popupRect = new Rect(position.x + PopupLeftPadding, y, position.width - PopupLeftPadding, lineHeight);
			EditorGUI.showMixedValue = isTypeMixed;

			var displayName = GetShortTypeName(property, isTypeMixed);
			if (GUI.Button(popupRect, displayName, EditorStyles.popup))
			{
				var popup = new ComponentSelectorPopup(property.Copy(), position.width - PopupLeftPadding);
				PopupWindow.Show(popupRect, popup);
			}
			EditorGUI.showMixedValue = false;

			if (property.managedReferenceValue == null)
			{
				DrawWarningIcon("This component is null or missing. You must remove it or select new one.");
				EditorGUI.EndProperty();
				return;
			}

			if (!isTypeMixed && HasDuplicateType(property))
			{
				// Draw warning.
				DrawWarningIcon("This component type is already added. Only one is allowed per entity.");
			}
			else if (property.isExpanded && !isTypeMixed && property.managedReferenceValue != null)
			{
				// Draw property.
				var fieldRect = new Rect(position.x + FoldoutOffset, y, position.width - FoldoutOffset, EditorGUI.GetPropertyHeight(property, true) - lineHeight);
				EditorGUI.PropertyField(fieldRect, property, GUIContent.none, true);
			}
			else if (!isTypeMixed)
			{
				// Draw foldout.
				var foldoutRect = new Rect(position.x + FoldoutOffset, y, position.width - FoldoutOffset, lineHeight);
				property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, GUIContent.none, false);
			}

			EditorGUI.EndProperty();

			void DrawWarningIcon(string tooltip)
			{
				var icon = EditorGUIUtility.IconContent("console.warnicon");
				icon.tooltip = tooltip;
				var iconRect = new Rect(position.x + WarningIconOffset, y, lineHeight, lineHeight);
				GUI.Label(iconRect, icon);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var height = EditorGUIUtility.singleLineHeight;
			if (!IsTypeMixed(property) && !HasDuplicateType(property) && property.managedReferenceValue != null)
			{
				height = EditorGUI.GetPropertyHeight(property, true);
			}
			return height;
		}

		private static string GetShortTypeName(SerializedProperty property, bool isTypeMixed)
		{
			if (isTypeMixed)
			{
				return "\u2014"; // Dash.
			}

			if (property.managedReferenceValue == null)
			{
				return "<Select Component>";
			}

			var fullName = property.managedReferenceValue.GetType().FullName;
			var lastDot = fullName.LastIndexOf('.');
			return lastDot >= 0 ? fullName.Substring(lastDot + 1) : fullName;
		}

		public static bool IsTypeMixed(SerializedProperty property)
		{
			var path = property.propertyPath;
			Type firstType = null;
			var initialized = false;

			foreach (var target in property.serializedObject.targetObjects)
			{
				var so = new SerializedObject(target);
				var p = so.FindProperty(path);
				var value = p.managedReferenceValue;

				if (value == null)
				{
					if (initialized && firstType != null)
					{
						return true;
					}

					firstType = null;
					initialized = true;
					continue;
				}

				var type = value.GetType();

				if (!initialized)
				{
					firstType = type;
					initialized = true;
				}
				else if (type != firstType)
				{
					return true;
				}
			}

			return false;
		}

		public static bool HasDuplicateType(SerializedProperty elementProperty)
		{
			if (elementProperty == null || elementProperty.managedReferenceValue == null)
			{
				return false;
			}

			var listProperty = SerializedPropertyUtils.GetOwningList(elementProperty);
			if (listProperty == null || !listProperty.isArray)
			{
				return false;
			}

			var targetType = elementProperty.managedReferenceValue.GetType();

			for (int i = 0; i < listProperty.arraySize; i++)
			{
				var sibling = listProperty.GetArrayElementAtIndex(i);

				if (SerializedPropertyUtils.IsSameProperty(sibling, elementProperty))
				{
					continue;
				}

				if (sibling.managedReferenceValue?.GetType() == targetType)
				{
					return true;
				}
			}

			return false;
		}
	}
}
#endif
