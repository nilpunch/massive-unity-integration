using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Massive.Unity
{
	public class SceneRegistry : MonoBehaviour
	{
		[SerializeField] private ViewDataBaseConfig _viewConfig;
		[SerializeField] private bool _reactiveSynchronization = true;

		private UnityEntitySynchronization _unityEntitySynchronization;
		private IRegistry _registry;

		private void Awake()
		{
			_registry = new Registry();

			foreach (var monoEntity in SceneManager.GetActiveScene().GetRootGameObjects()
				         .SelectMany(root => root.GetComponentsInChildren<MonoEntity>()))
			{
				monoEntity.ApplyToRegistry(_registry);
				Destroy(monoEntity.gameObject);
			}

			_unityEntitySynchronization = new UnityEntitySynchronization(_registry, new ViewPool(_viewConfig), _reactiveSynchronization);
		}

		private void OnDestroy()
		{
			_unityEntitySynchronization.Dispose();
		}

		private void LateUpdate()
		{
			if (!_reactiveSynchronization)
			{
				_unityEntitySynchronization.SynchronizeComponents();
				_unityEntitySynchronization.SyncronizeEntities();
				_unityEntitySynchronization.SynchronizeViews();
			}
		}
	}
}