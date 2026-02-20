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

			if (!property.isExpanded)
			{
				EditorGUI.EndProperty();
				return;
			}

			EditorGUI.indentLevel++;

			var contentRect = new Rect(
				position.x,
				position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
				position.width,
				EditorGUIUtility.singleLineHeight
			);

			if (ViewDataBase.Instance == null)
			{
				EditorGUI.LabelField(contentRect, "View DB not found.");
				EditorGUI.indentLevel--;
				EditorGUI.EndProperty();
				return;
			}

			var idProp = property.FindPropertyRelative(nameof(ViewAsset.IdPlusOne));

			EditorGUI.showMixedValue = idProp.hasMultipleDifferentValues;

			EntityView currentView = null;

			if (!idProp.hasMultipleDifferentValues && property.managedReferenceValue != null)
			{
				var viewAsset = (ViewAsset)property.managedReferenceValue;
				currentView = ViewDataBase.Instance.GetViewPrefabOrNull(viewAsset);
			}

			EditorGUI.BeginChangeCheck();

			var newObject = (EntityView)EditorGUI.ObjectField(
				contentRect,
				"Prefab",
				currentView,
				typeof(EntityView),
				false
			);

			if (EditorGUI.EndChangeCheck())
			{
				ApplyToAllTargets(property, newObject);
			}

			EditorGUI.showMixedValue = false;

			EditorGUI.indentLevel--;
			EditorGUI.EndProperty();
		}

		private static void ApplyToAllTargets(SerializedProperty property, EntityView newView)
		{
			foreach (var target in property.serializedObject.targetObjects)
			{
				var so = new SerializedObject(target);
				var prop = so.FindProperty(property.propertyPath);

				if (newView == null)
				{
					prop.managedReferenceValue = null;
				}
				else
				{
					var newAsset = ViewDataBase.Instance.GetViewAsset(newView);
					if (newAsset.IsValid)
					{
						prop.managedReferenceValue = newAsset;
					}
					else
					{
						Debug.LogWarning("Selected EntityView is not registered in View DB.");
					}
				}

				so.ApplyModifiedProperties();
			}
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
