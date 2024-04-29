using System.Linq;
using Massive;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UPR
{
	public class SceneRegistry : MonoBehaviour
	{
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

			_unityEntitySynchronization = new UnityEntitySynchronization(_registry);
		}

		private void OnDestroy()
		{
			_unityEntitySynchronization.Dispose();
		}
	}
}
