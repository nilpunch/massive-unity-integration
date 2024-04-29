using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Massive.Unity
{
	public static class FileSceneRegistryUtils
	{
		public const string RegistryFileName = "Registry.data";

		public static string GetPathToSceneRegistry(Scene scene)
		{
			var assetDirectory = Path.GetDirectoryName(Application.dataPath)!;
			var sceneDirectory = Path.GetDirectoryName(scene.path)!;
			var sceneName = Path.GetFileNameWithoutExtension(scene.path);
			var pathToFile = Path.Combine(assetDirectory, sceneDirectory, sceneName, RegistryFileName);

			return pathToFile;
		}
	}
}
