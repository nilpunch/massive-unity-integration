using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Massive.Unity
{
	[CustomEditor(typeof(MonoComponentsView), true)]
	public class MonoComponentsViewInspector : Editor
	{
		private readonly List<IDataSet> _usedDataSets = new List<IDataSet>();
		private SerializedProperty _dummyComponents;

		private void OnEnable()
		{
			_dummyComponents = serializedObject.FindProperty(nameof(MonoComponentsView.DummyComponents));
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			var view = (MonoComponentsView)target;
			if (view.Registry is null || !view.Registry.IsAlive(view.Entity))
			{
				return;
			}

			EditorGUI.indentLevel++;
			_usedDataSets.Clear();
			for (var i = 0; i < view.Registry.SetRegistry.All.Length; i++)
			{
				var set = view.Registry.SetRegistry.All[i];

				if (view.Registry.SetRegistry.All[i] is not IDataSet dataSet)
				{
					continue;
				}

				if (!set.IsAssigned(view.Entity.Id))
				{
					continue;
				}

				var componentType = dataSet.GetDataType();

				_usedDataSets.Add(dataSet);

				if (_dummyComponents.arraySize < _usedDataSets.Count)
				{
					_dummyComponents.InsertArrayElementAtIndex(_usedDataSets.Count - 1);
				}

				var componentName = componentType.GetGenericName();
				var componentValue = dataSet.GetRaw(view.Entity.Id);

				var arrayElement = _dummyComponents.GetArrayElementAtIndex(_usedDataSets.Count - 1);
				
				EditorGUILayout.BeginVertical(GUI.skin.box);
				if (TryDrawBuiltIn(componentType, componentName, ref componentValue))
				{
					arrayElement.boxedValue = componentValue;
				}
				else
				{
					arrayElement.boxedValue = componentValue;
					EditorGUILayout.PropertyField(arrayElement, new GUIContent(componentName), true);
				}
				EditorGUILayout.EndVertical();
			}
			for (int i = _usedDataSets.Count; i < _dummyComponents.arraySize; i++)
			{
				_dummyComponents.DeleteArrayElementAtIndex(i);
			}

			EditorGUI.indentLevel--;

			serializedObject.ApplyModifiedPropertiesWithoutUndo();

			for (int i = 0; i < _usedDataSets.Count; i++)
			{
				_usedDataSets[i].SetRaw(view.Entity.Id, view.DummyComponents[i]);
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
