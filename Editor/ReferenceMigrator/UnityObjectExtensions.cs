#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace Massive.Unity.Editor
{
	internal static class UnityObjectExtensions
	{
		public static int GetLocalIdentifierInFile(this UnityObject unityObject)
		{
			PropertyInfo inspectorModeInfo = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);
			SerializedObject serializedObject = new SerializedObject(unityObject);
			inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);
			SerializedProperty localIdProp = serializedObject.FindProperty("m_LocalIdentfierInFile");
			return localIdProp.intValue;
		}
	}
}
#endif