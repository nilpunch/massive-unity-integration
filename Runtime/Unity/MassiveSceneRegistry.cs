using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Massive.Unity
{
	public class MassiveSceneRegistry : MonoBehaviour
	{
		[SerializeField] private ViewDataBaseConfig _viewConfig;
		[SerializeField] private bool _synchronizeEntities = true;
		[SerializeField] private bool _synchronizeComponents = true;
		[SerializeField] private bool _synchronizeViews = true;

		[SerializeField, Min(0)] private int _framesCapacity = 120;
		[SerializeField, Min(0)] private int _resimulations = 120;
		[SerializeField, Min(1)] private int _simulationFrequency = 60;
		[SerializeField, Min(1)] private int _saveEachNthFrame = 3;

		[SerializeField] private bool _drawDebugGUI = false;

		private UpdateSystem[] _updateSystems;
		private RollbackUpdateSystem[] _rollbackSystems;
		private UnityEntitySynchronization _unityEntitySynchronization;
		private MassiveRegistry _registry;
		private float _elapsedTime;
		private int _currentFrame;
		private Stopwatch _stopwatch;

		private void Awake()
		{
			_registry = new MassiveRegistry(new MassiveRegistryConfig() { FramesCapacity = _framesCapacity + 1 });
			_stopwatch = new Stopwatch();

			foreach (var monoEntity in SceneManager.GetActiveScene().GetRootGameObjects()
				         .SelectMany(root => root.GetComponentsInChildren<MonoEntity>())
				         .Where(monoEntity => monoEntity.gameObject.activeInHierarchy))
			{
				monoEntity.ApplyToRegistry(_registry);
				Destroy(monoEntity.gameObject);
			}

			_registry.SaveFrame();

			_updateSystems = FindObjectsOfType<UpdateSystem>();
			_rollbackSystems = _updateSystems.OfType<RollbackUpdateSystem>().ToArray();
			foreach (var updateSystem in _updateSystems)
			{
				updateSystem.Init(_registry);
			}

			_unityEntitySynchronization = new UnityEntitySynchronization(_registry, new EntityViewPool(_viewConfig));

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
			_stopwatch.Restart();

			if (_resimulations > 0)
			{
				int currentFrameCompressed = _currentFrame / _saveEachNthFrame;

				int targetCompressedFrame = Mathf.Max(_currentFrame - _resimulations, 0) / _saveEachNthFrame;

				int compressedFramesToRollback = currentFrameCompressed - targetCompressedFrame;

				compressedFramesToRollback = Mathf.Min(compressedFramesToRollback, _registry.CanRollbackFrames);

				_currentFrame = (currentFrameCompressed - compressedFramesToRollback) * _saveEachNthFrame;
				_registry.Rollback(compressedFramesToRollback);
				foreach (var rollbackSystem in _rollbackSystems)
				{
					rollbackSystem.Rollback(compressedFramesToRollback);
				}
			}

			_elapsedTime += Time.deltaTime;

			int targetFrame = Mathf.RoundToInt(_elapsedTime * _simulationFrequency);
			float deltaTime = 1f / _simulationFrequency;

			_debugResimulations = targetFrame - _currentFrame;

			while (_currentFrame < targetFrame)
			{
				foreach (var updateSystem in _updateSystems)
				{
					updateSystem.UpdateFrame(deltaTime);
				}

				_currentFrame++;
				if (_currentFrame % _saveEachNthFrame == 0)
				{
					_registry.SaveFrame();
					foreach (var rollbackSystem in _rollbackSystems)
					{
						rollbackSystem.SaveFrame();
					}
				}
			}

			_debugSimulationMs = _stopwatch.ElapsedMilliseconds;
			_stopwatch.Restart();

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

			_debugSynchronizationMs = _stopwatch.ElapsedMilliseconds;
		}

		private long _debugSimulationMs;
		private long _debugSynchronizationMs;
		private int _debugResimulations;

		private void OnGUI()
		{
			if (!_drawDebugGUI)
			{
				return;
			}

			float fontScaling = Screen.height / (float)1080;

			GUILayout.TextField($"{_debugSimulationMs}ms Simulation", new GUIStyle() { fontSize = Mathf.RoundToInt(70 * fontScaling), normal = new GUIStyleState() { textColor = Color.white } });
			GUILayout.TextField($"{_debugSynchronizationMs}ms Synchronization",
				new GUIStyle() { fontSize = Mathf.RoundToInt(50 * fontScaling), normal = new GUIStyleState() { textColor = Color.white } });
			GUILayout.TextField($"{_debugResimulations} Resimulations",
				new GUIStyle() { fontSize = Mathf.RoundToInt(50 * fontScaling), normal = new GUIStyleState() { textColor = Color.white } });
		}
	}
}
