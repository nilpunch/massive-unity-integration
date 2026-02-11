#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	[CustomEditor(typeof(ScriptableConfig), true)]
	internal class ScriptableConfigEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			if (DrawSetMainInstanceButton())
			{
				EditorGUILayout.Space();
			}

			DrawDefaultInspector();
		}

		private bool DrawSetMainInstanceButton()
		{
			var scriptableConfig = target as ScriptableConfig;
			if (scriptableConfig == null)
			{
				return false;
			}

			if (scriptableConfig.IsMainInstance)
			{
				return false;
			}

			if (GUILayout.Button("Set As Main Instance", GUILayout.Height(25)))
			{
				scriptableConfig.SetAsMainInstance();
			}

			return true;
		}
	}
}
#endif