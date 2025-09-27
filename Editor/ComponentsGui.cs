using System;
using System.Collections.Generic;
using System.Linq;
using Massive.Unity.Editor.UnityInternalBridge;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Massive.Unity.Editor
{
	public static class ComponentsGui
	{
		public static void DrawTypeSelector(Rect rect, SerializedProperty property)
		{
			var propertyType = property.managedReferenceValue?.GetType();
			var typeName = GetShortTypeName(propertyType);
			var typeNameContent = new GUIContent(typeName);

			if (EditorGUI.DropdownButton(rect, typeNameContent, FocusType.Passive))
			{
				var dropdown = new ReferenceTypeDropDown(property, new AdvancedDropdownState());
				dropdown.Show(rect);

				if (dropdown.CanHideHeader)
				{
					AdvancedDropdownProxy.SetShowHeader(dropdown, false);
				}

				Event.current.Use();
			}
		}

		private class ReferenceTypeDropDown : CustomAdvancedDropdown
		{
			private readonly SerializedProperty _property;
			private readonly Type _propertyType;

			public bool CanHideHeader { get; private set; }

			public ReferenceTypeDropDown(SerializedProperty property, AdvancedDropdownState state) : base(state)
			{
				_property = property;
				_propertyType = _property.managedReferenceValue?.GetType();
				minimumSize = new Vector2(5, 5);
			}

			protected override AdvancedDropdownItem BuildRoot()
			{
				var types = Components.AllComponentTypes;

				var groupByNamespace = types.Length > 20;

				CanHideHeader = !groupByNamespace;

				var root = new ReferenceTypeGroupItem("Type");

				foreach (var type in types)
				{
					IEnumerable<string> namespaceEnumerator = groupByNamespace && type.Namespace != null
						? type.Namespace.Split('.')
						: Array.Empty<string>();

					root.AddTypeChild(type, namespaceEnumerator.GetEnumerator());
				}

				root.Build();

				return root;
			}

			protected override void ItemSelected(AdvancedDropdownItem item)
			{
				if (!(item is ReferenceTypeItem referenceTypeItem))
				{
					return;
				}

				var serializedObject = _property.serializedObject;
				var propertyPath = _property.propertyPath;
				var newType = referenceTypeItem.Type;

				foreach (var target in serializedObject.targetObjects)
				{
					var so = new SerializedObject(target);
					var p = so.FindProperty(propertyPath);
					var currentType = p.managedReferenceValue?.GetType();

					if (currentType != newType)
					{
						p.managedReferenceValue = Activator.CreateInstance(newType!);
						so.ApplyModifiedProperties();
					}
				}
			}

			private class ReferenceTypeGroupItem : CustomAdvancedDropdownItem
			{
				private static readonly Texture2D ScriptIcon = EditorGUIUtility.FindTexture("cs Script Icon");

				private readonly List<ReferenceTypeItem> _childItems = new List<ReferenceTypeItem>();

				private readonly Dictionary<string, ReferenceTypeGroupItem> _childGroups =
					new Dictionary<string, ReferenceTypeGroupItem>();

				public ReferenceTypeGroupItem(string name) : base(name)
				{
				}

				public void AddTypeChild(Type type, IEnumerator<string> namespaceRemaining)
				{
					if (!namespaceRemaining.MoveNext())
					{
						_childItems.Add(new ReferenceTypeItem(type, ScriptIcon));
						return;
					}

					var ns = namespaceRemaining.Current ?? "";

					if (!_childGroups.TryGetValue(ns, out var child))
					{
						_childGroups[ns] = child = new ReferenceTypeGroupItem(ns);
					}

					child.AddTypeChild(type, namespaceRemaining);
				}

				public void Build()
				{
					foreach (var child in _childGroups.Values.OrderBy(it => it.name))
					{
						AddChild(child);

						child.Build();
					}

					AddSeparator();

					foreach (var child in _childItems)
					{
						AddChild(child);
					}
				}
			}

			private class ReferenceTypeItem : CustomAdvancedDropdownItem
			{
				private readonly GUIContent TypeContent;
				private readonly GUIContent NamespaceContent;

				public ReferenceTypeItem(Type type, Texture2D preview = null)
					: base(string.Empty, GetFullTypeName(type))
				{
					Type = type;
					icon = preview;

					TypeContent = new GUIContent(GetShortTypeName(type), preview);

					var ns = type == null ? string.Empty : Components.GetNamespace(type);
					NamespaceContent = string.IsNullOrWhiteSpace(ns) ? GUIContent.none : new GUIContent(" (" + ns + ")");
				}

				public Type Type { get; }

				public override void DrawItem(Rect rect, GUIStyle mainStyle, bool isHover, bool isActive, bool on, bool hasKeyboardFocus)
				{
					var typeStyle = new GUIStyle(mainStyle)
					{
						normal = { textColor = new Color(0.85f, 0.85f, 0.85f) },
						hover = { textColor = Color.white },
						active = { textColor = new Color(0.85f, 0.85f, 0.85f) },
						focused = { textColor = Color.white },
					};

					var namespaceStyle = UnityStyles.lineStyleFaint;

					Vector2 typeSize = typeStyle.CalcSize(TypeContent);

					Rect typeRect = new Rect(rect.x, rect.y, rect.width, rect.height);
					typeStyle.Draw(typeRect, TypeContent, isHover, isActive, on, hasKeyboardFocus);

					Rect namespaceRect = new Rect(rect.x + typeSize.x, rect.y, rect.width - typeSize.x, rect.height);
					if (NamespaceContent != GUIContent.none)
					{
						namespaceStyle.Draw(namespaceRect, NamespaceContent, on || hasKeyboardFocus, false, false, false);
					}
				}
			}
		}

		private static string GetShortTypeName(Type propertyType)
		{
			if (propertyType == null)
			{
				return "<Select Component>";
			}

			return Components.GetShortName(propertyType);
		}

		private static string GetFullTypeName(Type propertyType)
		{
			if (propertyType == null)
			{
				return "<Select Component>";
			}

			return Components.GetFullName(propertyType);
		}
	}
}
