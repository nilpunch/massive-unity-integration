using UnityEngine;

namespace Massive.Unity
{
	[DisallowMultipleComponent]
	public class EntityProvider : MonoBehaviour
	{
		public void ApplyToRegistry(ServiceLocator serviceLocator)
		{
			var entity = serviceLocator.Find<Registry>().CreateEntity();

			foreach (var monoComponent in GetComponents<ComponentProvider>())
			{
				monoComponent.ApplyToEntity(serviceLocator, entity);
			}
		}
	}
}
