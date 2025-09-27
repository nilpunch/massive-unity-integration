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

			CollectPresentTypes(property);

			var optionalLabel = insideArray ? GUIContent.none : label;
			if (property.managedReferenceValue == null)
			{
				var iconRect = SerializeReferenceGui.DrawPropertySelectorOnly(position, optionalLabel, property, ComponentTypeFilter);
				EditorUtils.DrawWarningIcon(iconRect, "This component is null or missing.");
				return;
			}

			var isTypeMixed = SerializedPropertyUtils.IsTypeMixed(property);

			if (!isTypeMixed && HasDuplicateType(property))
			{
				var iconRect = SerializeReferenceGui.DrawPropertySelectorOnly(position, optionalLabel, property, ComponentTypeFilter);
				EditorUtils.DrawWarningIcon(iconRect, "This component type is already added. Only one is allowed per entity.");
				return;
			}

			SerializeReferenceGui.DrawPropertyWithFoldout(position, optionalLabel, property, ComponentTypeFilter);

			bool ComponentTypeFilter(Type type)
			{
				return type.IsDefined(typeof(ComponentAttribute), false) && !_presentTypes.Contains(type);
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

		public static void CollectPresentTypes(SerializedProperty listOrValueProperty)
		{
			_presentTypes.Clear();

			var path = listOrValueProperty.propertyPath;

			foreach (var target in listOrValueProperty.serializedObject.targetObjects)
			{
				var so = new SerializedObject(target);
				var property = so.FindProperty(path);

				var listProperty = SerializedPropertyUtils.GetOwningList(property);
				if (listProperty != null && listProperty.isArray)
				{
					for (int i = 0; i < listProperty.arraySize; i++)
					{
						var sibling = listProperty.GetArrayElementAtIndex(i);

						if (sibling.managedReferenceValue != null)
						{
							_presentTypes.Add(sibling.managedReferenceValue.GetType());
						}
					}
					continue;
				}

				if (property != null && property.managedReferenceValue != null)
				{
					_presentTypes.Add(property.managedReferenceValue.GetType());
				}
			}
		}
	}
}
#endif
