using System;
using System.Collections.Generic;
using System.Linq;
using Massive.Unity.Editor.UnityInternalBridge;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Massive.Unity.Editor
{
	public static class SerializeReferenceGui
	{
		private const float PopupLeftPadding = 8f;
		private const int FoldoutOffsetSingle = 2;
		private const int IconOffset = 17;

		/// <summary>
		/// Returns the icon rect for additional indication.
		/// </summary>
		public static Rect DrawPropertyWithFoldout(Rect position, GUIContent label, SerializedProperty property, Func<Type, bool> typeMatch, string nullOptionText = "<Select>")
		{
			EditorGUI.BeginProperty(position, label, property);

			var propertyType = property.managedReferenceValue?.GetType();

			var popupLeftPadding = PopupLeftPadding;

			if (!string.IsNullOrWhiteSpace(label.text))
			{
				var labelContent = EditorGUIUtility.TrTextContent(label.text);
				var labelSize = EditorStyles.label.CalcSize(labelContent);

				var labelRect = new Rect(position.x, position.y, labelSize.x, EditorGUIUtility.singleLineHeight);
				popupLeftPadding = labelSize.x + 18f;
				EditorGUI.PrefixLabel(labelRect, label);
			}

			var lineHeight = EditorGUIUtility.singleLineHeight;

			var isTypeMixed = SerializedPropertyUtils.IsTypeMixed(property);

			// Draw popup.
			var popupRect = new Rect(position.x + popupLeftPadding, position.y, position.width - popupLeftPadding, lineHeight);
			EditorGUI.showMixedValue = isTypeMixed;
			SerializeReferenceGui.DrawPropertyTypeSelector(popupRect, property, typeMatch, nullOptionText);
			EditorGUI.showMixedValue = false;

			// Check for null.
			if (property.managedReferenceValue == null)
			{
				EditorGUI.EndProperty();
				return new Rect(position.x + popupLeftPadding - IconOffset, position.y, lineHeight, lineHeight);
			}

			// Draw the property.
			var popupOffset = popupLeftPadding - FoldoutOffsetSingle;
			if (ReflectionUtils.HasAnyFields(propertyType))
			{
				if (property.isExpanded && !isTypeMixed && property.managedReferenceValue != null)
				{
					// Draw property.
					var fieldRect = new Rect(position.x + popupOffset, position.y, position.width - popupOffset, EditorGUI.GetPropertyHeight(property, true) - lineHeight);
					EditorGUI.PropertyField(fieldRect, property, GUIContent.none, true);
				}
				else if (!isTypeMixed)
				{
					// Draw foldout.
					var foldoutRect = new Rect(position.x + popupOffset, position.y, position.width - popupOffset, lineHeight);
					property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, GUIContent.none, false);
				}
			}
			EditorGUI.EndProperty();

			return new Rect(position.x + popupLeftPadding - IconOffset, position.y, lineHeight, lineHeight);
		}

		/// <summary>
		/// Returns the icon rect for additional indication.
		/// </summary>
		public static Rect DrawPropertySelectorOnly(Rect position, GUIContent label, SerializedProperty property, Func<Type, bool> typeMatch, string nullOptionText = "<Select>")
		{
			EditorGUI.BeginProperty(position, label, property);

			var popupLeftPadding = PopupLeftPadding;

			if (!string.IsNullOrWhiteSpace(label.text))
			{
				var labelContent = EditorGUIUtility.TrTextContent(label.text);
				var labelSize = EditorStyles.label.CalcSize(labelContent);

				var labelRect = new Rect(position.x, position.y, labelSize.x, EditorGUIUtility.singleLineHeight);
				popupLeftPadding = labelSize.x + 18f;
				EditorGUI.PrefixLabel(labelRect, label);
			}

			var lineHeight = EditorGUIUtility.singleLineHeight;

			var isTypeMixed = SerializedPropertyUtils.IsTypeMixed(property);

			// Draw popup.
			var popupRect = new Rect(position.x + popupLeftPadding, position.y, position.width - popupLeftPadding, lineHeight);
			EditorGUI.showMixedValue = isTypeMixed;
			SerializeReferenceGui.DrawPropertyTypeSelector(popupRect, property, typeMatch, nullOptionText);
			EditorGUI.showMixedValue = false;

			EditorGUI.EndProperty();

			return new Rect(position.x + popupLeftPadding - IconOffset, position.y, lineHeight, lineHeight);
		}

		public static float GetHeightWithFoldout(SerializedProperty property)
		{
			var height = EditorGUIUtility.singleLineHeight;
			if (!SerializedPropertyUtils.IsTypeMixed(property) && property.managedReferenceValue != null)
			{
				height = EditorGUI.GetPropertyHeight(property, true);
			}
			return height;
		}

		private const int ClosedDropdown = -1;
		private static int s_openedDropdown = ClosedDropdown;

		public static void DrawPropertyTypeSelector(Rect rect, SerializedProperty property, Func<Type, bool> typeMatch, string selectionName = "Type")
		{
			var propertyType = property.managedReferenceValue?.GetType();
			var typeName = propertyType == null ? $"<Select + {selectionName}>" : TypeUtils.GetShortName(propertyType);
			var fullTypeName = propertyType == null ? string.Empty : TypeUtils.GetFullName(propertyType);
			var typeNameContent = new GUIContent(typeName, fullTypeName);

			var dropdownKey = HashCode.Combine(
				property.serializedObject.targetObject.GetInstanceID(),
				property.propertyPath.GetHashCode());

			if (EditorGUI.DropdownButton(rect, typeNameContent, FocusType.Passive))
			{
				if (s_openedDropdown == dropdownKey)
				{
					s_openedDropdown = ClosedDropdown;
					Event.current.Use();
					return;
				}

				var dropdown = new ReferenceTypeDropDown((selectedType =>
				{
					var serializedObject = property.serializedObject;
					var propertyPath = property.propertyPath;
					var newType = selectedType;

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
				}), new AdvancedDropdownState(), typeMatch, selectionName);
				dropdown.Show(rect);

				if (dropdown.CanHideHeader)
				{
					AdvancedDropdownProxy.SetShowHeader(dropdown, false);
				}

				s_openedDropdown = dropdownKey;

				Event.current.Use();
			}
		}

		public static void DrawTypeSelector(Rect rect, Type currentType, Action<Type> typeCallback, Func<Type, bool> typeMatch, string selectionName = "<Select>")
		{
			var typeName = currentType == null ? $"<Select + {selectionName}>" : TypeUtils.GetShortName(currentType);
			var fullTypeName = currentType == null ? string.Empty : TypeUtils.GetFullName(currentType);
			var typeNameContent = new GUIContent(typeName, fullTypeName);

			if (EditorGUI.DropdownButton(rect, typeNameContent, FocusType.Passive))
			{
				if (s_openedDropdown != ClosedDropdown)
				{
					s_openedDropdown = ClosedDropdown;
					Event.current.Use();
					return;
				}

				var dropdown = new ReferenceTypeDropDown(typeCallback, new AdvancedDropdownState(), typeMatch, selectionName);
				dropdown.Show(rect);

				if (dropdown.CanHideHeader)
				{
					AdvancedDropdownProxy.SetShowHeader(dropdown, false);
				}

				s_openedDropdown = 1;

				Event.current.Use();
			}
		}

		private class ReferenceTypeDropDown : CustomAdvancedDropdown
		{
			private readonly Action<Type> _itemSelected;
			private readonly Func<Type, bool> _typeMatch;
			private readonly string _selectionName;
			private readonly string _nullOptionName;

			public bool CanHideHeader { get; private set; }

			public ReferenceTypeDropDown(Action<Type> itemSelected, AdvancedDropdownState state, Func<Type, bool> typeMatch, string selectionName = "Type") : base(state)
			{
				_typeMatch = typeMatch;
				_itemSelected = itemSelected;
				_selectionName = selectionName;
				_nullOptionName = $"<Select + {_selectionName}>";
				minimumSize = new Vector2(0, 60);
			}

			protected override AdvancedDropdownItem BuildRoot()
			{
				var types = TypeUtils
					.AllNonAbstractTypes
					.Where(type =>
						!typeof(UnityEngine.Object).IsAssignableFrom(type)
						&& type.IsDefined(typeof(SerializableAttribute), false)
						&& !type.Name.Contains("<")
						&& !type.IsGenericTypeDefinition
						&& _typeMatch.Invoke(type))
					.Where(type => type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null)
					.ToList();

				var groupByNamespace = types.Count > 20;

				CanHideHeader = !groupByNamespace;

				var root = new ReferenceTypeGroupItem(_selectionName, _nullOptionName);

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
				s_openedDropdown = ClosedDropdown;

				if (!(item is ReferenceTypeItem referenceTypeItem))
				{
					return;
				}

				_itemSelected?.Invoke(referenceTypeItem.Type);
			}

			private class ReferenceTypeGroupItem : CustomAdvancedDropdownItem
			{
				private static readonly Texture2D ScriptIcon = EditorGUIUtility.FindTexture("cs Script Icon");

				private readonly string _nullOptionText;
				private readonly List<ReferenceTypeItem> _childItems = new List<ReferenceTypeItem>();

				private readonly Dictionary<string, ReferenceTypeGroupItem> _childGroups =
					new Dictionary<string, ReferenceTypeGroupItem>();

				public ReferenceTypeGroupItem(string name, string nullOptionText) : base(name)
				{
					_nullOptionText = nullOptionText;
				}

				public void AddTypeChild(Type type, IEnumerator<string> namespaceRemaining)
				{
					if (!namespaceRemaining.MoveNext())
					{
						_childItems.Add(new ReferenceTypeItem(type, _nullOptionText, ScriptIcon));
						return;
					}

					var ns = namespaceRemaining.Current ?? "";

					if (!_childGroups.TryGetValue(ns, out var child))
					{
						_childGroups[ns] = child = new ReferenceTypeGroupItem(ns, _nullOptionText);
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

					foreach (var child in _childItems)
					{
						AddChild(child);
					}
				}
			}

			public class ReferenceTypeItem : CustomAdvancedDropdownItem
			{
				private readonly GUIContent TypeContent;
				private readonly GUIContent NamespaceContent;

				public ReferenceTypeItem(Type type, string nullOptionText, Texture2D preview = null)
					: base(string.Empty, type == null ? string.Empty : TypeUtils.GetFullName(type))
				{
					Type = type;
					icon = preview;

					TypeContent = new GUIContent(type == null ? nullOptionText : TypeUtils.GetShortName(type), preview);

					var ns = type == null ? string.Empty : TypeUtils.GetNamespace(type);
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
	}
}
