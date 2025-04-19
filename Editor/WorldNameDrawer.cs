#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	[CustomPropertyDrawer(typeof(WorldNameAttribute))]
	public class WorldNameDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.String)
			{
				EditorGUI.LabelField(position, label.text, "Use [WorldName] with string.");
				return;
			}

			var worldNames = EditorCache.WorldNames;
			if (worldNames.Length == 0)
			{
				EditorGUI.LabelField(position, label.text, "<No Worlds>");
				return;
			}

			// Show mixed if multiedit and different values.
			EditorGUI.showMixedValue = property.hasMultipleDifferentValues;

			var currentIndex = Array.IndexOf(worldNames, property.stringValue);
			if (currentIndex < 0)
			{
				currentIndex = 0;
			}

			// Draw label with short name.
			var fieldRect = EditorGUI.PrefixLabel(position, label);

			EditorGUI.BeginChangeCheck();
			var selectedIndex = EditorGUI.Popup(fieldRect, currentIndex, worldNames);
			if (EditorGUI.EndChangeCheck())
			{
				property.stringValue = worldNames[selectedIndex];
			}

			EditorGUI.showMixedValue = false;

			// Overlay short name if not mixed.
			if (!property.hasMultipleDifferentValues)
			{
				EditorGUI.LabelField(fieldRect, GetShortName(worldNames[selectedIndex]), EditorStyles.popup);
			}
		}

		private static string GetShortName(string fullName)
		{
			if (string.IsNullOrEmpty(fullName))
			{
				return "<None>";
			}
			var lastDot = fullName.LastIndexOf('.');
			return lastDot >= 0 ? fullName.Substring(lastDot + 1) : fullName;
		}
	}
}
#endif
