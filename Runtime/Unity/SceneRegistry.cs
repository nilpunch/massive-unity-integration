using System;
using System.Diagnostics;
using System.Linq;
using Massive.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Time = UnityEngine.Time;

namespace Massive.Unity
{
	public class SceneRegistry : MonoBehaviour
	{
		[SerializeField] private ViewDataBase _viewDataBase;
		[SerializeField] private bool _reactiveSynchronization = true;
		[SerializeField] protected bool _synchronizeViews = true;
		
		[SerializeField, Min(1)] private int _simulationFrequency = 60;
		[SerializeField] private bool _fixedTimeStep = false;

		private Session _session;
		private UpdateSystem[] _updateSystems;
		protected UnityEntitySynchronization _unityEntitySynchronization;
		protected Registry _registry;
		private int _currentFrame;

		private void Awake()
		{
			_session = new Session();
			
			_registry = _session.Registry;
			_session.Services.Assign(_viewDataBase);

			foreach (var monoEntity in SceneManager.GetActiveScene().GetRootGameObjects()
				         .SelectMany(root => root.GetComponentsInChildren<EntityProvider>())
				         .Where(monoEntity => monoEntity.gameObject.activeInHierarchy))
			{
				monoEntity.ApplyToRegistry(_session.Services);
				Destroy(monoEntity.gameObject);
			}

			_updateSystems = FindObjectsOfType<UpdateSystem>();
			foreach (var updateSystem in _updateSystems)
			{
				updateSystem.Init(_session.Services);
			}
			
			_unityEntitySynchronization = new UnityEntitySynchronization(_registry, new EntityViewPool(_viewDataBase));

			if (_synchronizeViews)
			{
				_unityEntitySynchronization.SynchronizeViews();
				
				if (_reactiveSynchronization)
				{
					_unityEntitySynchronization.SubscribeViews();
				}
			}
		}

		private void OnDestroy()
		{
			_unityEntitySynchronization.Dispose();
		}

		private void Update()
		{
			_stopwatch.Restart();

			if (_fixedTimeStep)
			{
				int targetFrame = Mathf.RoundToInt(Time.time * _simulationFrequency);
				float deltaTime = 1f / _simulationFrequency;
			
				while (_currentFrame < targetFrame)
				{
					foreach (var updateSystem in _updateSystems)
					{
						updateSystem.UpdateFrame(deltaTime);
					}
			
					_currentFrame++;
				}
			}
			else
			{
				foreach (var updateSystem in _updateSystems)
				{
					updateSystem.UpdateFrame(Time.deltaTime);
				}
			}

			const int averageCount = 10;
			
			_debugSimulationMs = _debugSimulationMs * (averageCount - 1) / averageCount + _stopwatch.ElapsedMilliseconds / (float)averageCount;
			_stopwatch.Restart();

			if (!_reactiveSynchronization)
			{
				if (_synchronizeViews)
				{
					_unityEntitySynchronization.SynchronizeViews();
				}
			}
			
			_debugSynchronizationMs = _stopwatch.ElapsedMilliseconds;
		}
		
		private float _debugSimulationMs;
		private float _debugSynchronizationMs;
		private Stopwatch _stopwatch = new Stopwatch();

		private void OnGUI()
		{
			float fontScaling = Screen.height / (float)1080;

			GUILayout.TextField($"{_debugSimulationMs:F1}ms Simulation", new GUIStyle() { fontSize = Mathf.RoundToInt(70 * fontScaling), normal = new GUIStyleState() { textColor = Color.white } });
			GUILayout.TextField($"{_debugSynchronizationMs}ms Synchronization", new GUIStyle() { fontSize = Mathf.RoundToInt(50 * fontScaling), normal = new GUIStyleState() { textColor = Color.white } });
		}
	}
}
