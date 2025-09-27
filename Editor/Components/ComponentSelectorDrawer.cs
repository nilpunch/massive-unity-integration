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
		private const float PopupLeftPadding = 8f;

		private const int WarningIconOffset = 17;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var insideArray = SerializedPropertyUtils.IsArrayElement(property);

			var popupLeftPadding = PopupLeftPadding;
			var lineHeight = EditorGUIUtility.singleLineHeight;
			var y = position.y;
			if (!insideArray)
			{
				var labelContent = EditorGUIUtility.TrTextContent(label.text);
				var labelSize = EditorStyles.label.CalcSize(labelContent);
				popupLeftPadding = labelSize.x + 18f;
			}

			CollectPresentTypes(property);

			var optionalLabel = insideArray ? GUIContent.none : label;
			if (property.managedReferenceValue == null)
			{
				SerializeReferenceGui.DrawPropertySelectorOnly(position, optionalLabel, property, ComponentTypeFilter);
				DrawWarningIcon("This component is null or missing.");
				return;
			}

			var isTypeMixed = SerializedPropertyUtils.IsTypeMixed(property);
			
			if (!isTypeMixed && HasDuplicateType(property))
			{
				SerializeReferenceGui.DrawPropertySelectorOnly(position, optionalLabel, property, ComponentTypeFilter);
				DrawWarningIcon("This component type is already added. Only one is allowed per entity.");
				return;
			}

			SerializeReferenceGui.DrawPropertyWithFoldout(position, optionalLabel, property, ComponentTypeFilter);

			bool ComponentTypeFilter(Type type)
			{
				return type.IsDefined(typeof(ComponentAttribute), false) && !_presentTypes.Contains(type);
			}

			void DrawWarningIcon(string tooltip)
			{
				var iconOffset = popupLeftPadding - WarningIconOffset;

				var icon = EditorGUIUtility.IconContent("console.warnicon");
				icon.tooltip = tooltip;
				var iconRect = new Rect(position.x + iconOffset, y, lineHeight, lineHeight);
				GUI.Label(iconRect, icon);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var height = EditorGUIUtility.singleLineHeight;
			if (!SerializedPropertyUtils.IsTypeMixed(property) && !HasDuplicateType(property) && property.managedReferenceValue != null)
			{
				height = EditorGUI.GetPropertyHeight(property, true);
			}
			return height;
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

		private static HashSet<Type> _presentTypes = new HashSet<Type>();

		public static void CollectPresentTypes(SerializedProperty elementProperty)
		{
			_presentTypes.Clear();
			if (elementProperty == null || elementProperty.managedReferenceValue == null)
			{
				return;
			}

			_presentTypes.Add(elementProperty.managedReferenceValue.GetType());

			var listProperty = SerializedPropertyUtils.GetOwningList(elementProperty);
			if (listProperty == null || !listProperty.isArray)
			{
				return;
			}
			
			for (int i = 0; i < listProperty.arraySize; i++)
			{
				var sibling = listProperty.GetArrayElementAtIndex(i);

				if (sibling.managedReferenceValue != null)
				{
					_presentTypes.Add(sibling.managedReferenceValue.GetType());
				}
			}
		}
	}
}
#endif
