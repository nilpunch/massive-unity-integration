using UnityEngine;
using UnityEngine.SceneManagement;

namespace Massive.Unity
{
	public class FileRegistry : MonoBehaviour
	{
		[SerializeField] private RegistryParserConfig _parserConfig;
		[SerializeField] private ViewDataBaseConfig _viewConfig;

		private ViewPool _viewPool;
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

			_unityEntitySynchronization = new UnityEntitySynchronization(_registry, new ViewPool(_viewConfig));
		}

		private void OnDestroy()
		{
			_unityEntitySynchronization.Dispose();
		}
	}
}
