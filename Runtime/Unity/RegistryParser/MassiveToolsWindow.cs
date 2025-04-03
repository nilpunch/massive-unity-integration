#if UNITY_EDITOR
using System.Linq;
using Massive.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Massive.Unity
{
	public class MassiveToolsWindow : EditorWindow
	{
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

			EditorGUILayout.Space(5f);
			if (GUILayout.Button("Save Scene Registry"))
			{
				SaveSceneRegistry();
			}
			if (GUILayout.Button("Open save path"))
			{
				OpenInWin(FileSceneRegistryUtils.GetPathToSceneRegistry(SceneManager.GetActiveScene()));
			}
		}

		private void SaveSceneRegistry()
		{
			var registrySerializer = new RegistrySerializer();

			var registry = new World();
			var serviceLocator = new ServiceLocator();
			serviceLocator.Assign(registry);

			var activeScene = SceneManager.GetActiveScene();
			var entityProviders = SceneManager.GetActiveScene().GetRootGameObjects()
				.SelectMany(root => root.GetComponentsInChildren<EntityProvider>())
				.Where(monoEntity => monoEntity.gameObject.activeInHierarchy).ToArray();
			
			foreach (var entityProvider in entityProviders)
			{
				entityProvider.ApplyEntity(serviceLocator);
			}
			
			foreach (var entityProvider in entityProviders)
			{
				entityProvider.ApplyComponents(serviceLocator);
				Destroy(entityProvider.gameObject);
			}

			RegistryFileUtils.WriteToFile(FileSceneRegistryUtils.GetPathToSceneRegistry(activeScene), registry, registrySerializer);

			AssetDatabase.Refresh();
		}

		public static void OpenInWin(string path)
		{
			bool openInsidesOfFolder = false;

			// try windows
			string winPath = path.Replace("/", "\\"); // windows explorer doesn't like forward slashes

			if ( System.IO.Directory.Exists(winPath) ) // if path requested is a folder, automatically open insides of that folder
				openInsidesOfFolder = true;

			try
			{
				System.Diagnostics.Process.Start("explorer.exe", (openInsidesOfFolder ? "/root," : "/select,") + "\"" + winPath + "\"");
			}
			catch ( System.ComponentModel.Win32Exception e )
			{
				e.HelpLink = "";
			}
		}
	}
}
#endif
