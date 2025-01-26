#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Massive.Unity
{
	[CustomPropertyDrawer(typeof(UnfoldAttribute))]
	public class UnfoldDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, true)
				-  EditorGUIUtility.singleLineHeight
				- EditorGUIUtility.standardVerticalSpacing;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty child = property.Copy();
			var currentPosition = position.position;
			if (child.NextVisible(true))
			{
				do
				{
					var childName = new GUIContent(child.displayName);
					var propertyHeight = EditorGUI.GetPropertyHeight(child, childName, true);
					var childPosition = new Rect(currentPosition, new Vector2(position.size.x, propertyHeight));

					EditorGUI.BeginProperty(childPosition, childName, child);
					EditorGUI.PropertyField(childPosition, child, childName);
					EditorGUI.EndProperty();

					currentPosition.y += propertyHeight + EditorGUIUtility.standardVerticalSpacing;
				} while (child.NextVisible(false));
			}
		}
	}
}
#endif