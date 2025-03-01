using UnityEngine;

namespace Massive.Unity
{
	[DisallowMultipleComponent]
	public class EntityProvider : MonoBehaviour
	{
		public Entity Entity { get; private set; }

		public void ApplyEntity(ServiceLocator serviceLocator)
		{
			Entity = serviceLocator.Find<Registry>().CreateEntity();
		}

		public void ApplyComponents(ServiceLocator serviceLocator)
		{
			foreach (var monoComponent in GetComponents<ComponentProvider>())
			{
				monoComponent.ApplyToEntity(serviceLocator, Entity);
			}
		}
	}
}
