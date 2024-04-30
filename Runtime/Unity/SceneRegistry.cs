using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Massive.Unity
{
	public class SceneRegistry : MonoBehaviour
	{
		[SerializeField] private ViewDataBaseConfig _viewConfig;

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

			_unityEntitySynchronization = new UnityEntitySynchronization(_registry, new ViewDataBase(_viewConfig));
		}

		private void OnDestroy()
		{
			_unityEntitySynchronization.Dispose();
		}
	}
}
