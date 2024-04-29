using Massive;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UPR
{
	public class FileSceneRegistry : MonoBehaviour
	{
		[SerializeField] private RegistryParserConfig _parserConfig;

		private UnityEntitySynchronization _unityEntitySynchronization;
		private IRegistry _registry;

		private void Awake()
		{
			foreach (var monoEntity in FindObjectsOfType<MonoEntity>())
			{
				Destroy(monoEntity.gameObject);
			}

			var pathToSceneRegistry = FileSceneRegistryUtils.GetPathToSceneRegistry(SceneManager.GetActiveScene());

			_registry = RegistryFileUtils.ReadFromFile(pathToSceneRegistry, _parserConfig.CreateParser());

			_unityEntitySynchronization = new UnityEntitySynchronization(_registry);
		}

		private void OnDestroy()
		{
			_unityEntitySynchronization.Dispose();
		}
	}
}
