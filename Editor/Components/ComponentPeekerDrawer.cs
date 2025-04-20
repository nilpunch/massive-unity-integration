#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	[CustomPropertyDrawer(typeof(ComponentPeekerAttribute), true)]
	public class ComponentPeekerDrawer : PropertyDrawer
	{
		private const float LeftPopupPadding = 10f;
		private const int FoldoutOffset = 5;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var lineHeight = EditorGUIUtility.singleLineHeight;
			var y = position.y;

			var isTypeMixed = IsTypeMixed(property);

			// Draw popup
			var popupRect = new Rect(position.x + LeftPopupPadding, y, position.width - LeftPopupPadding, lineHeight);
			EditorGUI.showMixedValue = isTypeMixed;

			var displayName = GetShortTypeName(property, isTypeMixed);
			if (GUI.Button(popupRect, displayName, EditorStyles.popup))
			{
				var popup = new ComponentPickerPopup(property.Copy(), position.width - LeftPopupPadding);
				PopupWindow.Show(popupRect, popup);
			}
			EditorGUI.showMixedValue = false;

			if (property.isExpanded && !isTypeMixed && property.managedReferenceValue != null)
			{
				// Draw property.
				var fieldRect = new Rect(position.x + FoldoutOffset, y, position.width - FoldoutOffset, EditorGUI.GetPropertyHeight(property, true) - lineHeight - 2);
				EditorGUI.PropertyField(fieldRect, property, GUIContent.none, true);
			}
			else
			{
				// Draw foldout.
				var foldoutRect = new Rect(position.x + FoldoutOffset, y, position.width - FoldoutOffset, lineHeight);
				property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, GUIContent.none, false);
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var height = EditorGUIUtility.singleLineHeight;
			if (!IsTypeMixed(property) && property.managedReferenceValue != null)
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

		private static bool IsTypeMixed(SerializedProperty property)
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
	}
}
#endif
