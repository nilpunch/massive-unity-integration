using System.Diagnostics;
using System.Linq;
using Massive.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Time = UnityEngine.Time;

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

		private SimulationAdapter _adapter;
		private InputSystem[] _inputSystems;
		private UnityEntitySynchronization _unityEntitySynchronization;
		private Session _session;
		private MassiveWorld _world;
		private float _elapsedTime;
		private Stopwatch _stopwatch;
		private SimulationTicksTracker _simulationTicksTracker = new SimulationTicksTracker();

		private void Awake()
		{
			_session = new Session(new SessionConfig(_simulationFrequency, _saveEachNthTick, worldConfig: new MassiveWorldConfig(framesCapacity: _framesCapacity + 1)));

			_world = _session.World;
			_session.Services.Assign(_viewDataBase);

			_adapter = new SimulationAdapter(_session.Time);
			_session.Simulations.Add(_adapter);
			_session.Simulations.Add(_simulationTicksTracker);

			_stopwatch = new Stopwatch();

			var entityProviders = SceneManager.GetActiveScene().GetRootGameObjects()
				.SelectMany(root => root.GetComponentsInChildren<EntityProvider>())
				.Where(monoEntity => monoEntity.gameObject.activeInHierarchy).ToArray();
			
			foreach (var entityProvider in entityProviders)
			{
				entityProvider.ApplyEntity(_session.Services);
			}
			
			foreach (var entityProvider in entityProviders)
			{
				entityProvider.ApplyComponents(_session.Services);
				Destroy(entityProvider.gameObject);
			}


			_inputSystems = FindObjectsOfType<InputSystem>();
			foreach (var inputSystem in _inputSystems)
			{
				inputSystem.Init(_session.Inputs);
			}
			foreach (var updateSystem in FindObjectsOfType<UpdateSystem>())
			{
				updateSystem.Init(_session.Services);
				_adapter.Systems.Add(updateSystem);
			}

			_world.SaveFrame();
			
			_unityEntitySynchronization = new UnityEntitySynchronization(_world, new EntityViewPool(_viewDataBase));

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

			foreach (var inputSystem in _inputSystems)
			{
				inputSystem.UpdateInput(targetTick);
			}

			_simulationTicksTracker.Restart();
			_stopwatch.Restart();

			_session.ChangeTracker.NotifyChange(Mathf.Max(0, targetTick - _resimulations));
			_session.Loop.FastForwardToTick(targetTick);

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
