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

		[SerializeField, Min(1)] private int _simulationFrequency = 60;
		
		private UpdateSystem[] _updateSystems;
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
			
			_updateSystems = FindObjectsOfType<UpdateSystem>();
			foreach (var updateSystem in _updateSystems)
			{
				updateSystem.Init(_registry);
			}

			_unityEntitySynchronization = new UnityEntitySynchronization(_registry, new EntityViewPool(_viewConfig), _reactiveSynchronization);

			if (_synchronizeEntities)
			{
				_unityEntitySynchronization.SynchronizeEntities();
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
			float deltaTime = Mathf.RoundToInt(1f / _simulationFrequency);
			
			foreach (var updateSystem in _updateSystems)
			{
				updateSystem.UpdateFrame(deltaTime);
			}

			if (!_reactiveSynchronization)
			{
				if (_synchronizeComponents) // Synchronize components first to remove all components from dying entity
				{
					_unityEntitySynchronization.SynchronizeComponents();
				}
				if (_synchronizeEntities)
				{
					_unityEntitySynchronization.SynchronizeEntities();
				}
				if (_synchronizeViews)
				{
					_unityEntitySynchronization.SynchronizeViews();
				}
			}
		}
	}
}
