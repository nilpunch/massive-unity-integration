using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Massive.Unity
{
	[CustomEditor(typeof(MonoComponentsView), true)]
	[CanEditMultipleObjects]
	public class MonoComponentsViewInspector : Editor
	{
		private readonly List<SparseSet> _toUnassign = new List<SparseSet>();
		private readonly List<SparseSet> _commonSets = new List<SparseSet>();
		private readonly List<IDataSet> _commonDataSets = new List<IDataSet>();

		public override void OnInspectorGUI()
		{
			_commonSets.Clear();
			var setRegistry = ((MonoComponentsView)targets.FirstOrDefault(target => ((MonoComponentsView)target).Registry != null))?.Registry.SetRegistry;
			if (setRegistry == null)
			{
				return;
			}

			var allSets = setRegistry.All;
			for (int i = 0; i < allSets.Length; i++)
			{
				_commonSets.Add(allSets[i]);
			}
			foreach (MonoComponentsView target in targets)
			{
				if (target.Registry is null || !target.Registry.IsAlive(target.Entity))
				{
					continue;
				}
				
				for (int i = _commonSets.Count - 1; i >= 0; i--)
				{
					if (!_commonSets[i].IsAssigned(target.Entity.Id))
					{
						_commonSets.RemoveAt(i);
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

			foreach (MonoComponentsView target in targets)
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
			var serializedDummies = serializedObject.FindProperty(nameof(MonoComponentsView.DummyComponents));
			var buttonIcon = EditorGUIUtility.IconContent("d_winbtn_win_close_h@2x");
			var buttonStyle = new GUIStyle() { padding = new RectOffset() };

			_toUnassign.Clear();
			EditorGUI.indentLevel++;
			for (var i = 0; i < _commonSets.Count; i++)
			{
				var commonSet = _commonSets[i];
				if (commonSet is not IDataSet dataSet)
				{
					EditorGUILayout.BeginVertical(GUI.skin.box);
					Rect controlRect = EditorGUILayout.GetControlRect(true);
					Rect fieldRect = new Rect(controlRect.x, controlRect.y, controlRect.width - EditorGUIUtility.singleLineHeight, controlRect.height);
					Rect buttonRect = new Rect(controlRect.x + controlRect.width - EditorGUIUtility.singleLineHeight, controlRect.y, EditorGUIUtility.singleLineHeight, controlRect.height);
					EditorGUI.LabelField(fieldRect, setRegistry.TypeOf(commonSet).GetGenericName());
					if (GUI.Button(buttonRect, buttonIcon, buttonStyle))
					{
						_toUnassign.Add(commonSet);
					}
					EditorGUILayout.EndVertical();
					continue;
				}

				var componentType = dataSet.GetDataType();

				var componentName = componentType.GetGenericName();

				var arrayElement = serializedDummies.GetArrayElementAtIndex(_commonDataSets.IndexOf((IDataSet)commonSet));

				EditorGUI.showMixedValue = arrayElement.hasMultipleDifferentValues;
				EditorGUILayout.BeginVertical(GUI.skin.box);
				{
					Rect buttonRect;
					if (DrawBuiltInField(componentType, componentName, arrayElement, out var controlRect))
					{
						buttonRect = new Rect(controlRect.x + controlRect.width - EditorGUIUtility.singleLineHeight, controlRect.y,
							EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
						if (GUI.Button(buttonRect, buttonIcon, buttonStyle))
						{
							_toUnassign.Add(commonSet);
						}
					}
					else
					{
						controlRect = EditorGUILayout.GetControlRect(true, EditorGUI.GetPropertyHeight(arrayElement, true));
						buttonRect = new Rect(controlRect.x + controlRect.width - EditorGUIUtility.singleLineHeight, controlRect.y,
							EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
						if (GUI.Button(buttonRect, buttonIcon, buttonStyle))
						{
							_toUnassign.Add(commonSet);
						}
						EditorGUI.PropertyField(controlRect, arrayElement, new GUIContent(componentName), true);
					}
					
				}
				EditorGUILayout.EndVertical();
				EditorGUI.showMixedValue = false;
			}
			EditorGUI.indentLevel--;

			serializedObject.ApplyModifiedPropertiesWithoutUndo();

			foreach (var setToUnassign in _toUnassign)
			{
				foreach (MonoComponentsView target in targets)
				{
					setToUnassign.Unassign(target.Entity.Id);
				}
				var index = _commonDataSets.FindIndex(set => ReferenceEquals(set, setToUnassign));
				if (index >= 0)
				{
					_commonDataSets[index] = null;
				}
			}
			
			for (var i = 0; i < _commonDataSets.Count; i++)
			{
				var dataSet = _commonDataSets[i];
				if (dataSet == null)
				{
					continue;
				}
				foreach (MonoComponentsView target in targets)
				{
					dataSet.SetRaw(target.Entity.Id, target.DummyComponents[i]);
				}
			}

			bool DrawBuiltInField(Type type, string label, SerializedProperty serializedProperty, out Rect controlRect)
			{
				controlRect = default;
				
				var value = serializedProperty.boxedValue;
				if (type.IsEnum)
				{
					bool isFlags = Attribute.IsDefined(type, typeof(FlagsAttribute));
					serializedProperty.boxedValue = isFlags
						? EditorGUI.EnumFlagsField(CreateFieldRect(out controlRect), label, (Enum)value)
						: EditorGUI.EnumPopup(CreateFieldRect(out controlRect), label, (Enum)value);
					return true;
				}

				if (type == typeof(int))
				{
					serializedProperty.boxedValue = EditorGUI.IntField(CreateFieldRect(out controlRect), label, (int)value);
					return true;
				}
				else if (type == typeof(Vector3))
				{
					serializedProperty.boxedValue = EditorGUI.Vector3Field(CreateFieldRect(out controlRect), label, (Vector3)value);
					return true;
				}

				Rect CreateFieldRect(out Rect controlRect)
				{
					controlRect = EditorGUILayout.GetControlRect(true);
					Rect fieldRect = new Rect(controlRect.x, controlRect.y, controlRect.width - EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
					return fieldRect;
				}

				return false;
			}
		}
	}
}
