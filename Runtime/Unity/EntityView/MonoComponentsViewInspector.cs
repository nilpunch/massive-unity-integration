#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditor;

namespace Massive.Unity
{
	[CustomEditor(typeof(ComponentsView), true)]
	[CanEditMultipleObjects]
	public class MonoComponentsViewInspector : Editor
	{
		private readonly List<SparseSet> _toUnassign = new List<SparseSet>();
		private readonly List<SparseSet> _commonSets = new List<SparseSet>();
		private readonly List<IDataSet> _commonDataSets = new List<IDataSet>();

		public override void OnInspectorGUI()
		{
			_commonSets.Clear();
			var setRegistry = ((ComponentsView)targets.FirstOrDefault(target => ((ComponentsView)target).Registry != null))?.Registry.SetRegistry;
			if (setRegistry == null)
			{
				return;
			}

			var allSets = setRegistry.All;
			for (int i = 0; i < allSets.Length; i++)
			{
				_commonSets.Add(allSets[i]);
			}

			for (int i = _commonSets.Count - 1; i >= 0; i--)
			{
				foreach (ComponentsView target in targets)
				{
					if (target.Registry is null || !target.Registry.IsAlive(target.Entity))
					{
						continue;
					}

					if (!_commonSets[i].IsAssigned(target.Entity.Id))
					{
						_commonSets.RemoveAt(i);
						break;
					}
				}
			}

			_commonDataSets.Clear();
			foreach (var commonSet in _commonSets)
			{
				if (commonSet is IDataSet dataSet)
				{
					_commonDataSets.Add(dataSet);
				}
			}

			foreach (ComponentsView target in targets)
			{
				for (var i = 0; i < _commonDataSets.Count; i++)
				{
					var dataSet = _commonDataSets[i];

					if (target.DummyComponents.Count < i + 1)
					{
						target.DummyComponents.Insert(i, null);
					}
					
					if (target.Registry is null || !target.Registry.IsAlive(target.Entity))
					{
						target.DummyComponents[i] = null;
					}
					else
					{
						target.DummyComponents[i] = dataSet.GetRaw(target.Entity.Id);
					}
				}

				if (target.DummyComponents.Count > _commonDataSets.Count)
				{
					target.DummyComponents.RemoveRange(_commonDataSets.Count, target.DummyComponents.Count - _commonDataSets.Count);
				}
			}

			serializedObject.Update();
			var serializedDummies = serializedObject.FindProperty(nameof(ComponentsView.DummyComponents));
			var buttonIcon = EditorGUIUtility.IconContent("d_winbtn_win_close_h@2x");
			var buttonStyle = new GUIStyle() { padding = new RectOffset() };

			_toUnassign.Clear();
			EditorGUI.indentLevel++;
			EditorGUI.BeginChangeCheck();
			for (var i = 0; i < _commonSets.Count; i++)
			{
				EditorGUILayout.BeginVertical(GUI.skin.box);

				var commonSet = _commonSets[i];
				IDataSet dataSet = commonSet as IDataSet;

				// Define control rect
				Rect controlRect;
				if (dataSet is null)
				{
					controlRect = EditorGUILayout.GetControlRect(true);
				}
				else
				{
					var arrayElement = serializedDummies.GetArrayElementAtIndex(_commonDataSets.IndexOf(dataSet));
					controlRect = GetPropertyControlRect(dataSet.DataType, arrayElement);
				}

				// Draw button before any properties top made it clickable
				if (GUI.Button(GetButtonRect(controlRect), buttonIcon, buttonStyle))
				{
					_toUnassign.Add(commonSet);
				}

				// Draw everything
				if (dataSet is null)
				{
					EditorGUI.LabelField(GetSingleLineFieldRect(controlRect), setRegistry.TypeOf(commonSet).GetGenericName());
				}
				else
				{
					var componentType = dataSet.DataType;
					var componentName = componentType.GetGenericName();
					var arrayElement = serializedDummies.GetArrayElementAtIndex(_commonDataSets.IndexOf(dataSet));

					EditorGUI.showMixedValue = arrayElement.hasMultipleDifferentValues;
					if (HasBuiltInDrawer(componentType))
					{
						DrawBuiltInField(controlRect, componentType, componentName, arrayElement);
					}
					else
					{
						EditorGUI.PropertyField(controlRect, arrayElement, new GUIContent(componentName), true);
					}
					EditorGUI.showMixedValue = false;
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUI.indentLevel--;

			foreach (var setToUnassign in _toUnassign)
			{
				foreach (ComponentsView target in targets)
				{
					setToUnassign.Unassign(target.Entity.Id);
				}
				var index = _commonDataSets.FindIndex(set => ReferenceEquals(set, setToUnassign));
				if (index >= 0)
				{
					_commonDataSets[index] = null;
				}
			}

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedPropertiesWithoutUndo();

				for (var i = 0; i < _commonDataSets.Count; i++)
				{
					var dataSet = _commonDataSets[i];
					if (dataSet == null)
					{
						continue;
					}
					foreach (ComponentsView target in targets)
					{
						dataSet.SetRaw(target.Entity.Id, target.DummyComponents[i]);
					}
				}
			}

			bool HasBuiltInDrawer(Type type)
			{
				return type.IsEnum || type.IsPrimitive || type.IsArray
				       || type == typeof(Vector3);
			}

			Rect GetPropertyControlRect(Type type, SerializedProperty serializedProperty)
			{
				if (HasBuiltInDrawer(type))
				{
					return EditorGUILayout.GetControlRect(true);
				}
				else
				{
					return EditorGUILayout.GetControlRect(true, EditorGUI.GetPropertyHeight(serializedProperty, true));
				}
			}

			void DrawBuiltInField(Rect controlRect, Type type, string label, SerializedProperty serializedProperty)
			{
				var value = serializedProperty.boxedValue;
				if (type.IsEnum)
				{
					bool isFlags = Attribute.IsDefined(type, typeof(FlagsAttribute));
					serializedProperty.boxedValue = isFlags
						? EditorGUI.EnumFlagsField(GetSingleLineFieldRect(controlRect), label, (Enum)value)
						: EditorGUI.EnumPopup(GetSingleLineFieldRect(controlRect), label, (Enum)value);
				}
				else if (type == typeof(int))
				{
					serializedProperty.boxedValue = EditorGUI.IntField(GetSingleLineFieldRect(controlRect), label, (int)value);
				}
				else if (type == typeof(Vector3))
				{
					serializedProperty.boxedValue = EditorGUI.Vector3Field(GetSingleLineFieldRect(controlRect), label, (Vector3)value);
				}
				else
				{
					EditorGUI.LabelField(GetSingleLineFieldRect(controlRect), label + " (no drawer)");
				}
			}

			Rect GetSingleLineFieldRect(Rect controlRect)
			{
				return new Rect(controlRect.x, controlRect.y,
					controlRect.width - EditorGUIUtility.singleLineHeight - EditorGUIUtility.standardVerticalSpacing,
					EditorGUIUtility.singleLineHeight);
			}
			
			Rect GetButtonRect(Rect controlRect)
			{
				return new Rect(controlRect.x + controlRect.width - EditorGUIUtility.singleLineHeight, controlRect.y,
					EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
			}
		}
	}
}
#endif