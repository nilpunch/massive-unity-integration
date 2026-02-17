#if UNITY_EDITOR
using Massive.QoL;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	[CustomPropertyDrawer(typeof(ViewAsset))]
	internal class ViewAssetDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

			if (property.isExpanded)
			{
				EditorGUI.indentLevel++;

				var contentRect = new Rect(
					position.x,
					position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
					position.width,
					EditorGUIUtility.singleLineHeight
				);

				if (ViewDataBase.Instance == null)
				{
					EditorGUI.LabelField(contentRect, "ViewDataBase not found");
					EditorGUI.indentLevel--;
					EditorGUI.EndProperty();
					return;
				}

				var viewAsset = (ViewAsset)property.managedReferenceValue;
				var entityView = ViewDataBase.Instance.GetViewPrefabOrNull(viewAsset);

				var newObject = EditorGUI.ObjectField(contentRect, "Prefab", entityView, typeof(EntityView), false);

				if (newObject != entityView)
				{
					if (newObject == null)
					{
						property.managedReferenceValue = null;
					}
					else
					{
						var newAsset = ViewDataBase.Instance.GetViewAsset((EntityView)newObject);
						if (newAsset.IsValid)
						{
							property.managedReferenceValue = newAsset;
						}
						else
						{
							Debug.LogWarning($"Selected EntityView is not registered in ViewDataBase");
						}
					}

					property.serializedObject.ApplyModifiedProperties();
				}

				EditorGUI.indentLevel--;
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (!property.isExpanded)
			{
				return EditorGUIUtility.singleLineHeight;
			}

			return EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;
		}
	}
}
#endif
