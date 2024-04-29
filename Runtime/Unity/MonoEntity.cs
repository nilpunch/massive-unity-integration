using Massive;
using UnityEngine;

namespace Massive.Unity
{
	[DisallowMultipleComponent]
	public class MonoEntity : MonoBehaviour
	{
		public Entity Entity { get; private set; }

		public void ApplyToRegistry(IRegistry registry)
		{
			var entity = registry.CreateEntity();

			foreach (var monoComponent in GetComponents<MonoComponent>())
			{
				monoComponent.ApplyToEntity(registry, entity);
			}
		}

		public void Synchronize(Entity entity)
		{
			Entity = entity;
		}
	}
}
