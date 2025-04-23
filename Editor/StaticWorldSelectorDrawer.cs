#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	[CustomPropertyDrawer(typeof(StaticWorldSelectorAttribute))]
	public class StaticWorldSelectorDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.String)
			{
				EditorGUI.LabelField(position, label.text, "Use [StaticWorldSelector] with string.");
				return;
			}

			if (EditorCache.WorldNames.Length == 0)
			{
				EditorGUI.LabelField(position, label.text, "<No Worlds>");
				return;
			}

			// Show mixed if multiedit and different values.
			EditorGUI.showMixedValue = property.hasMultipleDifferentValues;

			var worldIndex = Array.BinarySearch(EditorCache.WorldNames, property.stringValue);
			var worldFound = worldIndex >= 0;

			// Draw label with short name.
			var fieldRect = EditorGUI.PrefixLabel(position, label);

			EditorGUI.BeginChangeCheck();
			var selectedIndex = EditorGUI.Popup(fieldRect, worldIndex, EditorCache.FormatedWorldNames);
			if (EditorGUI.EndChangeCheck())
			{
				property.stringValue = EditorCache.WorldNames[selectedIndex];
			}

			EditorGUI.showMixedValue = false;

			// Overlay short name if not mixed.
			if (!property.hasMultipleDifferentValues)
			{
				var displayText = worldFound
					? GetShortName(EditorCache.WorldNames[selectedIndex])
					: string.IsNullOrWhiteSpace(property.stringValue)
						? $"<Missing>"
						: $"<Missing: {property.stringValue}>";

				EditorGUI.LabelField(fieldRect, displayText, EditorStyles.popup);
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
