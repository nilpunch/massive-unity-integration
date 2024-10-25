using System.IO;
using Massive.Serialization;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

namespace Massive.Unity
{
	public class FileRegistry : SceneRegistry
	{
		private void OnGUI()
		{
			float fontScaling = Screen.height / (float)1080;
			var guiStyle = new GUIStyle(GUI.skin.button);
			guiStyle.fontSize = Mathf.RoundToInt(70 * fontScaling);

			if (GUILayout.Button("Save Registry", guiStyle))
			{
				var pathToSceneRegistry = FileSceneRegistryUtils.GetPathToSceneRegistry(SceneManager.GetActiveScene());

				Profiler.BeginSample("Serialize to file.");
				RegistryFileUtils.WriteToFile(pathToSceneRegistry, _registry, new RegistrySerializer());
				Profiler.EndSample();
			}

			if (GUILayout.Button("Load Registry", guiStyle))
			{
				var pathToSceneRegistry = FileSceneRegistryUtils.GetPathToSceneRegistry(SceneManager.GetActiveScene());

				if (File.Exists(pathToSceneRegistry))
				{
					Profiler.BeginSample("Deserialize from file.");
					RegistryFileUtils.ReadFromFile(pathToSceneRegistry, _registry, new RegistrySerializer());
					Profiler.EndSample();

					Profiler.BeginSample("Synchronize entities.");
					if (_synchronizeEntities)
					{
						_unityEntitySynchronization.SynchronizeEntities();
					}
					if (_synchronizeViews)
					{
						_unityEntitySynchronization.SynchronizeViews();
					}
					Profiler.EndSample();
				}
			}
		}
	}
}
