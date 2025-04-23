#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	[CustomPropertyDrawer(typeof(ComponentSelectorAttribute), false)]
	public class ComponentSelectorDrawer : PropertyDrawer
	{
		private const float PopupLeftPadding = 8f;

		private const int FoldoutOffset = 3;
		private const int WarningIconOffset = 17;

		private const int FoldoutOffsetSingle = 14;
		private const int WarningIconOffsetSingle = 17;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var propertyType = property.managedReferenceValue?.GetType();

			var popupLeftPadding = PopupLeftPadding;
			var insideArray = SerializedPropertyUtils.IsArrayElement(property);

			// Reserve label space only if not part of array.
			if (!insideArray)
			{
				var labelContent = EditorGUIUtility.TrTextContent(label.text);
				var labelSize = EditorStyles.label.CalcSize(labelContent);

				var labelRect = new Rect(position.x, position.y, labelSize.x, EditorGUIUtility.singleLineHeight);
				popupLeftPadding = labelSize.x + 18f;
				EditorGUI.PrefixLabel(labelRect, label);
			}

			var lineHeight = EditorGUIUtility.singleLineHeight;
			var y = position.y;

			var isTypeMixed = IsTypeMixed(property);

			// Draw popup.
			var popupRect = new Rect(position.x + popupLeftPadding, y, position.width - popupLeftPadding, lineHeight);
			EditorGUI.showMixedValue = isTypeMixed;
			var displayName = GetShortTypeName(propertyType, isTypeMixed);
			if (GUI.Button(popupRect, displayName, EditorStyles.popup))
			{
				var popup = new ComponentSelectorPopup(property.Copy(), position.width - popupLeftPadding);
				PopupWindow.Show(popupRect, popup);
			}
			EditorGUI.showMixedValue = false;

			// Check for null.
			if (property.managedReferenceValue == null)
			{
				DrawWarningIcon("This component is null or missing.");
				EditorGUI.EndProperty();
				return;
			}
			
			// Check for duplicates.
			if (property.managedReferenceValue == null)
			{
				DrawWarningIcon("This component is null or missing.");
				EditorGUI.EndProperty();
				return;
			}

			if (!isTypeMixed && HasDuplicateType(property))
			{
				DrawWarningIcon("This component type is already added. Only one is allowed per entity.");
				EditorGUI.EndProperty();
				return;
			}

			// Draw the property.
			var popupOffset = popupLeftPadding - (insideArray ? FoldoutOffset : FoldoutOffsetSingle);
			if (ReflectionUtils.HasAnyFields(propertyType))
			{
				if (property.isExpanded && !isTypeMixed && property.managedReferenceValue != null)
				{
					// Draw property.
					var fieldRect = new Rect(position.x + popupOffset, y, position.width - popupOffset, EditorGUI.GetPropertyHeight(property, true) - lineHeight);
					EditorGUI.PropertyField(fieldRect, property, GUIContent.none, true);
				}
				else if (!isTypeMixed)
				{
					// Draw foldout.
					var foldoutRect = new Rect(position.x + popupOffset, y, position.width - popupOffset, lineHeight);
					property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, GUIContent.none, false);
				}
			}
			EditorGUI.EndProperty();

			void DrawWarningIcon(string tooltip)
			{
				var iconOffset = popupLeftPadding - (insideArray ? WarningIconOffset : WarningIconOffsetSingle);

				var icon = EditorGUIUtility.IconContent("console.warnicon");
				icon.tooltip = tooltip;
				var iconRect = new Rect(position.x + iconOffset, y, lineHeight, lineHeight);
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

		private static string GetShortTypeName(Type propertyType, bool isTypeMixed)
		{
			if (isTypeMixed)
			{
				return "\u2014"; // Dash.
			}

			if (propertyType == null)
			{
				return "<Select Component>";
			}

			return Components.GetShortName(propertyType);
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
