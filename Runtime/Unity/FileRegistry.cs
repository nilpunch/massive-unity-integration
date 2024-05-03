using UnityEngine;
using UnityEngine.SceneManagement;

namespace Massive.Unity
{
	public class FileRegistry : MonoBehaviour
	{
		[SerializeField] private RegistryParserConfig _parserConfig;
		[SerializeField] private ViewDataBaseConfig _viewConfig;
		[SerializeField] private bool _reactiveSynchronization = true;
		[SerializeField] private bool _synchronizeEntities = true;
		[SerializeField] private bool _synchronizeComponents = true;
		[SerializeField] private bool _synchronizeViews = true;
		
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

			_unityEntitySynchronization = new UnityEntitySynchronization(_registry, new ViewPool(_viewConfig), _reactiveSynchronization);
			
			if (_synchronizeEntities)
			{
				_unityEntitySynchronization.SyncronizeEntities();
			}
			if (_synchronizeComponents)
			{
				_unityEntitySynchronization.SynchronizeComponents();
			}
			if (_synchronizeViews)
			{
				_unityEntitySynchronization.SynchronizeViews();
			}
		}

		private void OnDestroy()
		{
			_unityEntitySynchronization.Dispose();
		}

		private void Update()
		{
			if (!_reactiveSynchronization)
			{
				if (_synchronizeComponents) // Synchronize components first to remove all components from dying entity
				{
					_unityEntitySynchronization.SynchronizeComponents();
				}
				if (_synchronizeEntities)
				{
					_unityEntitySynchronization.SyncronizeEntities();
				}
				if (_synchronizeViews)
				{
					_unityEntitySynchronization.SynchronizeViews();
				}
			}
		}
	}
}
