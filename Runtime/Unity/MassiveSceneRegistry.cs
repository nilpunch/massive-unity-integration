using System.Diagnostics;
using System.Linq;
using Massive.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Massive.Unity
{
	public class MassiveSceneRegistry : MonoBehaviour
	{
		[SerializeField] private ViewDataBase _viewDataBase;
		[SerializeField] private bool _synchronizeViews = true;

		[SerializeField, Min(0)] private int _framesCapacity = 120;
		[SerializeField, Min(0)] private int _resimulations = 120;
		[SerializeField, Min(1)] private int _simulationFrequency = 60;
		[SerializeField, Min(1)] private int _saveEachNthTick = 5;

		[SerializeField] private bool _drawDebugGUI = false;

		private SimulationSystemAdapter _systemsAdapter;
		private UpdateSystem[] _updateSystems;
		private UnityEntitySynchronization _unityEntitySynchronization;
		private Simulation _simulation;
		private MassiveRegistry _registry;
		private float _elapsedTime;
		private int _currentFrame;
		private Stopwatch _stopwatch;
		private SimulationTicksTracker _simulationTicksTracker = new SimulationTicksTracker();

		private void Awake()
		{
			_simulation = new Simulation(_simulationFrequency, _saveEachNthTick, new MassiveRegistryConfig(framesCapacity: _framesCapacity + 1));

			_registry = _simulation.Registry;
			_registry.AssignService(_viewDataBase);

			_systemsAdapter = new SimulationSystemAdapter(_registry.Service<SimulationTime>());
			_simulation.Systems.Add(_systemsAdapter);
			_simulation.Systems.Add(_simulationTicksTracker);

			_stopwatch = new Stopwatch();

			foreach (var monoEntity in SceneManager.GetActiveScene().GetRootGameObjects()
				         .SelectMany(root => root.GetComponentsInChildren<EntityProvider>())
				         .Where(monoEntity => monoEntity.gameObject.activeInHierarchy))
			{
				monoEntity.ApplyToRegistry(_registry);
				Destroy(monoEntity.gameObject);
			}

			_registry.SaveFrame();

			_updateSystems = FindObjectsOfType<UpdateSystem>();
			foreach (var updateSystem in _updateSystems)
			{
				updateSystem.Init(_registry);
				_systemsAdapter.Systems.Add(updateSystem);
			}

			_unityEntitySynchronization = new UnityEntitySynchronization(_registry, new EntityViewPool(_viewDataBase));

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
			_elapsedTime += Time.deltaTime;
			int targetTick = Mathf.RoundToInt(_elapsedTime * _simulationFrequency);

			_simulationTicksTracker.Restart();
			_stopwatch.Restart();
			_simulation.TickChangeLog.NotifyChange(Mathf.Max(0, targetTick - _resimulations));
			_simulation.Loop.FastForwardToTick(targetTick);
			_debugSimulationMs = _stopwatch.ElapsedMilliseconds;
			_debugResimulations = _simulationTicksTracker.TicksAmount;

			_stopwatch.Restart();
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
