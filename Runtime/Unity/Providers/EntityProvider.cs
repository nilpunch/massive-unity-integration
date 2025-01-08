using UnityEngine;

namespace Massive.Unity
{
	[DisallowMultipleComponent]
	public class EntityProvider : MonoBehaviour
	{
		public void ApplyToRegistry(Registry registry)
		{
			var entity = registry.CreateEntity();

			foreach (var monoComponent in GetComponents<ComponentProvider>())
			{
				monoComponent.ApplyToEntity(registry, entity);
			}
		}
	}
}
