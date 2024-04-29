#if UNITY_EDITOR
using System;
using System.Linq;
using Massive;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Massive.Unity
{
	public class MassiveToolsWindow : EditorWindow
	{
		[SerializeField] private RegistryParserConfig _parserConfig;

		[MenuItem("Window/Massive ECS/Tools")]
		public static void ShowWindow()
		{
			MassiveToolsWindow wnd = GetWindow<MassiveToolsWindow>();
			wnd.titleContent = new GUIContent("Massive Tools");
			wnd.LoadData();
		}

		private void OnEnable()
		{
			LoadData();
		}

		private void OnDisable()
		{
			SaveData();
		}

		private void LoadData()
		{
			string data = EditorPrefs.GetString("MassiveToolsWindow" + GetInstanceID(), JsonUtility.ToJson(this, false));
			JsonUtility.FromJsonOverwrite(data, this);
		}

		private void SaveData()
		{
			string data = JsonUtility.ToJson(this, false);
			EditorPrefs.SetString("MassiveToolsWindow" + GetInstanceID(), data);
		}

		private void OnGUI()
		{
			EditorGUILayout.Space(10f);
			EditorGUILayout.LabelField("Massive Tools", new GUIStyle()
			{
				fontSize = 25, normal = new GUIStyleState()
				{
					textColor = Color.white
				},
				alignment = TextAnchor.MiddleCenter
			});
			EditorGUILayout.Space(10f);

			if (Application.isPlaying)
			{
				EditorGUILayout.LabelField("Exit Play Mode to use tools.");
				return;
			}

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Parser Config");
			_parserConfig = (RegistryParserConfig)EditorGUILayout.ObjectField(_parserConfig, typeof(RegistryParserConfig), false);
			EditorGUILayout.EndHorizontal();

			GUI.enabled = _parserConfig != null;

			EditorGUILayout.Space(5f);
			if (GUILayout.Button("Save Scene Registry"))
			{
				SaveSceneRegistry();
			}

			GUI.enabled = true;
		}

		private void SaveSceneRegistry()
		{
			if (_parserConfig == null)
			{
				ShowNotification(new GUIContent("No parser config found"));
				return;
			}

			var registryParser = _parserConfig.CreateParser();

			var registry = new Registry();

			var activeScene = SceneManager.GetActiveScene();
			foreach (var monoEntity in activeScene.GetRootGameObjects().SelectMany(root => root.GetComponentsInChildren<MonoEntity>()))
			{
				monoEntity.ApplyToRegistry(registry);
			}

			RegistryFileUtils.WriteToFile(FileSceneRegistryUtils.GetPathToSceneRegistry(activeScene), registry, registryParser);

			AssetDatabase.Refresh();
		}
	}
}
#endif
