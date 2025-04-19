#if UNITY_EDITOR
using UnityEditor;

namespace Massive.Unity.Editor
{
	// [CanEditMultipleObjects]
	// [CustomEditor(typeof(EntityProvider))]
	// internal class EntityProviderEditor : UnityEditor.Editor
	// {
	// 	private SerializedProperty _worldTypeName;
	//
	// 	private void OnEnable()
	// 	{
	// 		if (target == null)
	// 		{
	// 			return;
	// 		}
	//
	// 		_worldTypeName = serializedObject.FindProperty("_worldTypeName");
	// 	}
	//
	// 	public override void OnInspectorGUI()
	// 	{
	// 		serializedObject.Update();
	//
	// 		var worldNames = EditorCache.WorldNames;
	//
	// 		int currentIndex = 0;
	// 		if (!string.IsNullOrEmpty(_worldTypeName.stringValue))
	// 		{
	// 			for (int i = 0; i < worldNames.Length; i++)
	// 			{
	// 				if (worldNames[i] == _worldTypeName.stringValue)
	// 				{
	// 					currentIndex = i;
	// 					break;
	// 				}
	// 			}
	// 		}
	//
	// 		EditorGUI.BeginChangeCheck();
	// 		int selectedIndex = EditorGUILayout.Popup("World", currentIndex, worldNames);
	// 		if (EditorGUI.EndChangeCheck())
	// 		{
	// 			_worldTypeName.stringValue = worldNames[selectedIndex];
	// 		}
	//
	// 		serializedObject.ApplyModifiedProperties();
	// 	}
	// }
}
#endif
