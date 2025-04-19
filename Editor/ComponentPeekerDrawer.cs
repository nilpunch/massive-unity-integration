#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	[CustomPropertyDrawer(typeof(ComponentPeekerAttribute))]
	public class ComponentPeekerDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.ManagedReference)
			{
				EditorGUI.LabelField(position, label.text, "Use [ComponentPeeker] with List<object>.");
				return;
			}

			EditorGUI.LabelField(position, label.text, "WIP");
			return;
		}
	}
}
#endif