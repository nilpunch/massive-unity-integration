using UnityEngine;

namespace Massive.Unity
{
	[DisallowMultipleComponent]
	public class MonoEntity : MonoBehaviour
	{
		public void ApplyToRegistry(Registry registry)
		{
			var entity = registry.CreateEntity();

			foreach (var monoComponent in GetComponents<MonoComponent>())
			{
				monoComponent.ApplyToEntity(registry, entity);
			}
		}
	}
}
