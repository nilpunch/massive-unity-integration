#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	public class ComponentSelectorPopup : PopupWindowContent
	{
		private readonly SerializedProperty _property;
		private readonly float _desiredWidth;
		private string _search = "";
		private Vector2 _scroll;
		private bool _wantsFocus = true;
		private const string SearchControlName = "ComponentSelectorPopupSearchField";
		private const string SearchEditorPrefsKey = "ComponentSelectorPopupLastSearch";

		public ComponentSelectorPopup(SerializedProperty property, float width)
		{
			_property = property;
			_desiredWidth = width;
			_search = EditorPrefs.GetString(SearchEditorPrefsKey, "");
		}

		public override void OnClose()
		{
			EditorPrefs.SetString(SearchEditorPrefsKey, _search);
		}

		public override Vector2 GetWindowSize() => new Vector2(_desiredWidth, 300);

		public override void OnGUI(Rect rect)
		{
			if (_property == null)
			{
				EditorGUILayout.LabelField("No property.");
				return;
			}

			int controlId = GUIUtility.GetControlID(FocusType.Keyboard);

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
				{
					continue;
				}

				if (DrawStyledTypeButton(name, type))
				{
					break;
				}
			}

			EditorGUILayout.EndScrollView();
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
				var currentType = p.managedReferenceValue?.GetType();

				if (currentType != newType)
				{
					p.managedReferenceValue = newValue;
					so.ApplyModifiedProperties();
				}
			}
		}

		private bool DrawStyledTypeButton(string fullName, Type type)
		{
			var entryRect = GUILayoutUtility.GetRect(0, 20, GUILayout.ExpandWidth(true));

			var isHovered = entryRect.Contains(Event.current.mousePosition);
			var clicked = Event.current.type == EventType.MouseDown && isHovered;

			if (isHovered)
			{
				EditorGUI.DrawRect(entryRect, new Color(0.2392157f, 0.3764706f, 0.5686275f, 1f)); // Unity blue highlight.
			}

			var iconRect = new Rect(entryRect.x + 4, entryRect.y + 2, 16, 16);
			GUI.Label(iconRect, EditorGUIUtility.IconContent("cs Script Icon"));

			var shortName = Components.GetShortName(type);
			var ns = Components.GetNamespace(type);

			var shortContent = new GUIContent(shortName);
			var nsContent = string.IsNullOrWhiteSpace(ns) ? GUIContent.none : new GUIContent(" (" + ns + ")");

			var shortWidth = EditorStyles.label.CalcSize(shortContent).x;

			var shortRect = new Rect(iconRect.xMax + 4, entryRect.y + 2, shortWidth, 16);
			var nsRect = new Rect(shortRect.xMax, entryRect.y + 2, entryRect.width - shortRect.xMax - 4, 16);

			var typeColor = isHovered ? Color.white : new Color(0.85f, 0.85f, 0.85f);
			var nsColor = isHovered ? new Color(0.6f, 0.6f, 0.6f) : new Color(0.5f, 0.5f, 0.5f);

			var typeStyle = CreateFlatLabelStyle(typeColor);
			var nsStyle = CreateFlatLabelStyle(nsColor);

			GUI.Label(shortRect, shortContent, typeStyle);
			GUI.Label(nsRect, nsContent, nsStyle);

			if (clicked)
			{
				Event.current.Use();
				var newInstance = Activator.CreateInstance(type);
				ApplyToAllTargets(newInstance);
				editorWindow.Close();
				GUIUtility.ExitGUI();
				return true;
			}

			return false;
		}

		private void DrawSearchBar()
		{
			using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
			{
				GUI.SetNextControlName(SearchControlName);
				_search = GUILayout.TextField(_search, GUI.skin.FindStyle("ToolbarSearchTextField"));

				if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSearchCancelButton")))
				{
					_search = "";
					GUI.FocusControl(null);
				}
			}

			// Focus only once after window opens
			if (_wantsFocus)
			{
				EditorGUI.FocusTextInControl(SearchControlName);
				_wantsFocus = false;
			}
		}

		private static GUIStyle CreateFlatLabelStyle(Color color)
		{
			var style = new GUIStyle(EditorStyles.label)
			{
				fontStyle = FontStyle.Normal,
				richText = false,
				clipping = TextClipping.Clip,
				wordWrap = false,
				stretchWidth = false,
				alignment = TextAnchor.MiddleLeft,
				contentOffset = Vector2.zero
			};

			style.normal.textColor = color;
			style.hover.textColor = color;
			style.active.textColor = color;
			style.focused.textColor = color;
			style.onNormal = style.normal;
			style.onHover = style.normal;
			style.onActive = style.normal;
			style.onFocused = style.normal;

			return style;
		}
	}
}
#endif
