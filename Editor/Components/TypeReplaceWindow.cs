#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	public class TypeReplaceWindow : EditorWindow
	{
		private string missingTypeName = "";
		private Action<Type> callback;
		private Type selectedType;

		public static void Show(string missingTypeName, Action<Type> onComplete)
		{
			TypeReplaceWindow window = CreateInstance<TypeReplaceWindow>();
			window.titleContent = new GUIContent("Replace Missing Type");
			window.missingTypeName = missingTypeName;
			window.callback = onComplete;
			window.maxSize = new Vector2(1000, 120);
			window.minSize = new Vector2(400, 120);
			window.position = new Rect(Screen.width / 2f - window.minSize.x / 2, Screen.height / 2f - window.minSize.y / 2, window.minSize.x, window.minSize.y);
			window.Focus();
			window.ShowModalUtility();
		}

		private void OnTypeSelected(Type type) => selectedType = type;

		private bool MatchAny(Type type) => true;

		void OnGUI()
		{
			GUILayout.Space(5);

			GUILayout.Label("Select type to replace:");
			GUILayout.Label($"{missingTypeName}", EditorStyles.boldLabel);

			GUILayout.Space(5);

			var controlRect = EditorGUILayout.GetControlRect();
			SerializeReferenceGui.DrawTypeSelector(controlRect, selectedType, OnTypeSelected, MatchAny);

			// inputText = EditorGUILayout.TextField(inputText);

			GUILayout.Space(10);

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("OK"))
			{
				callback?.Invoke(selectedType);
				Close();
			}
			if (GUILayout.Button("Cancel"))
			{
				callback?.Invoke(null);
				Close();
			}
			GUILayout.EndHorizontal();
		}
	}
}
#endif