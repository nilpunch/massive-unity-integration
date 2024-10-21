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
		private readonly List<IDataSet> _commonSetIndices = new List<IDataSet>();
		private readonly List<SparseSet> _commonSets = new List<SparseSet>();
		private readonly List<IDataSet> _commonDataSets = new List<IDataSet>();
		private SerializedProperty _dummyComponents;

		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			_dummyComponents = serializedObject.FindProperty(nameof(MonoComponentsView.DummyComponents));

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
			foreach (MonoComponentsView target in serializedObject.targetObjects)
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

			foreach (MonoComponentsView target in serializedObject.targetObjects)
			{
				var targetObject = new SerializedObject(target);
				var dummyComponents = targetObject.FindProperty(nameof(MonoComponentsView.DummyComponents));
			
				for (var i = 0; i < _commonDataSets.Count; i++)
				{
					var dataSet = _commonDataSets[i];

					if (dummyComponents.arraySize < i + 1)
					{
						dummyComponents.InsertArrayElementAtIndex(i);
					}
					
					var arrayElement = dummyComponents.GetArrayElementAtIndex(i);

					if (target.Registry is null || !target.Registry.IsAlive(target.Entity))
					{
						arrayElement.boxedValue = null;
					}
					else
					{
						arrayElement.boxedValue = dataSet.GetRaw(target.Entity.Id);
					}
				}

				for (int i = dummyComponents.arraySize - 1; i >= _commonDataSets.Count; i--)
				{
					dummyComponents.DeleteArrayElementAtIndex(i);
				}

				targetObject.ApplyModifiedPropertiesWithoutUndo();
			}

			serializedObject.Update();

			EditorGUI.indentLevel++;
			for (var i = 0; i < _commonSets.Count; i++)
			{
				var commonSet = _commonSets[i];
				if (commonSet is not IDataSet dataSet)
				{
					EditorGUILayout.BeginVertical(GUI.skin.box);
					EditorGUILayout.LabelField(setRegistry.TypeOf(commonSet).GetGenericName());
					EditorGUILayout.EndVertical();
					continue;
				}

				var componentType = dataSet.GetDataType();

				var componentName = componentType.GetGenericName();

				var arrayElement = _dummyComponents.GetArrayElementAtIndex(_commonDataSets.IndexOf((IDataSet)commonSet));

				EditorGUI.showMixedValue = arrayElement.hasMultipleDifferentValues;
				EditorGUILayout.BeginVertical(GUI.skin.box);
				var boxedValue = arrayElement.boxedValue;
				if (TryDrawBuiltIn(componentType, componentName, ref boxedValue))
				{
					arrayElement.boxedValue = boxedValue;
				}
				else
				{
					EditorGUILayout.PropertyField(arrayElement, new GUIContent(componentName), true);
				}
				EditorGUILayout.EndVertical();
				EditorGUI.showMixedValue = false;
			}
			EditorGUI.indentLevel--;

			serializedObject.ApplyModifiedPropertiesWithoutUndo();
			
			foreach (MonoComponentsView target in serializedObject.targetObjects)
			{
				for (var i = 0; i < _commonDataSets.Count; i++)
				{
					var dataSet = _commonDataSets[i];
					dataSet.SetRaw(target.Entity.Id, target.DummyComponents[i]);
				}
			}

			bool TryDrawBuiltIn(Type type, string label, ref object value)
			{
				if (type.IsEnum)
				{
					bool isFlags = Attribute.IsDefined(type, typeof(FlagsAttribute));
					value = isFlags
						? EditorGUILayout.EnumFlagsField(label, (Enum)value)
						: EditorGUILayout.EnumPopup(label, (Enum)value);
					return true;
				}

				if (type == typeof(int))
				{
					value = EditorGUILayout.IntField(label, (int)value);
					return true;
				}
				else if (type == typeof(Vector3))
				{
					value = EditorGUILayout.Vector3Field(label, (Vector3)value);
					return true;
				}

				return false;
			}
		}
	}
}
