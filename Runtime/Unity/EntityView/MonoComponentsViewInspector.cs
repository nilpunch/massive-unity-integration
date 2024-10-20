using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity
{
	[CustomEditor(typeof(MonoComponentsView), true)]
	public class MonoComponentsViewInspector : Editor
	{
		private readonly List<IDataSet> _usedDataSets = new List<IDataSet>();
		private SerializedProperty _dummies;

		private void OnEnable()
		{
			_dummies = serializedObject.FindProperty(nameof(MonoComponentsView.DummyComponents));
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

				if (Attribute.GetCustomAttribute(componentType, typeof(SerializableAttribute)) is null)
				{
					continue;
				}

				_usedDataSets.Add(dataSet);

				if (_dummies.arraySize < _usedDataSets.Count)
				{
					_dummies.InsertArrayElementAtIndex(_usedDataSets.Count - 1);
				}

				var componentName = componentType.GetGenericName();
				var componentValue = dataSet.GetRaw(view.Entity.Id);

				var arrayElement = _dummies.GetArrayElementAtIndex(_usedDataSets.Count - 1);

				arrayElement.managedReferenceValue = componentValue;

				EditorGUILayout.BeginVertical(GUI.skin.box);
				EditorGUILayout.PropertyField(arrayElement, new GUIContent(componentName), true);
				EditorGUILayout.EndVertical();
			}
			for (int i = _dummies.arraySize; i < _usedDataSets.Count; i++)
			{
				_dummies.DeleteArrayElementAtIndex(i);
			}
			EditorGUI.indentLevel--;

			serializedObject.ApplyModifiedPropertiesWithoutUndo();

			for (int i = 0; i < _usedDataSets.Count; i++)
			{
				_usedDataSets[i].SetRaw(view.Entity.Id, view.DummyComponents[i]);
			}
		}
	}
}
