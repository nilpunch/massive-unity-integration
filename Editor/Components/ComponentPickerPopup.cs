#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	public class ComponentPickerPopup : PopupWindowContent
	{
		private readonly SerializedProperty _property;
		private readonly float _desiredWidth;
		private string _search = "";
		private Vector2 _scroll;

		public ComponentPickerPopup(SerializedProperty property, float width)
		{
			_property = property;
			_desiredWidth = width;
		}

		public override Vector2 GetWindowSize() => new Vector2(_desiredWidth, 300);

		public override void OnGUI(Rect rect)
		{
			if (_property == null)
			{
				EditorGUILayout.LabelField("No property.");
				return;
			}

			DrawSearchBar();

			EditorGUILayout.Space(4);
			_scroll = EditorGUILayout.BeginScrollView(_scroll);

			var names = Components.AllComponentNames;
			var types = Components.AllComponentTypes;

			for (var i = 0; i < types.Length; i++)
			{
				var name = names[i];
				var type = types[i];

				if (!string.IsNullOrEmpty(_search) &&
					!name.Contains(_search, StringComparison.OrdinalIgnoreCase))
					continue;

				if (GUILayout.Button(name, EditorStyles.miniButton))
				{
					var newInstance = Activator.CreateInstance(type);
					ApplyToAllTargets(newInstance);
					editorWindow.Close();
					GUIUtility.ExitGUI();
				}
			}

			EditorGUILayout.EndScrollView();

			EditorGUILayout.Space(2);
			if (GUILayout.Button("<None>", EditorStyles.miniButtonMid))
			{
				ApplyToAllTargets(null);
				editorWindow.Close();
				GUIUtility.ExitGUI();
			}
		}

		private void ApplyToAllTargets(object newValue)
		{
			var serializedObject = _property.serializedObject;
			var propertyPath = _property.propertyPath;
			var newType = newValue?.GetType();

			foreach (var target in serializedObject.targetObjects)
			{
				var so = new SerializedObject(target);
				var p = so.FindProperty(propertyPath);
				var current = p.managedReferenceValue;
				var currentType = current?.GetType();

				if (currentType != newType)
				{
					p.managedReferenceValue = newValue;
					so.ApplyModifiedProperties();
				}
			}
		}

		private void DrawSearchBar()
		{
			using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
			{
				_search = GUILayout.TextField(_search, GUI.skin.FindStyle("ToolbarSearchTextField"));

				if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSearchCancelButton")))
				{
					_search = "";
					GUI.FocusControl(null);
				}
			}
		}
	}
}
#endif
