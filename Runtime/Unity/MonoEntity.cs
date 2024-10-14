using UnityEngine;

namespace Massive.Unity
{
	[DisallowMultipleComponent]
	public class MonoEntity : MonoBehaviour
	{
		private Registry _registry;

		public Entity Entity { get; private set; }

		public void ApplyToRegistry(Registry registry)
		{
			var entity = registry.CreateEntity();

			foreach (var monoComponent in GetComponents<MonoComponent>())
			{
				monoComponent.ApplyToEntity(registry, entity);
			}
		}

		public void Synchronize(Registry registry, Entity entity)
		{
			_registry = registry;
			Entity = entity;
		}

		public void DestroyEntity()
		{
			if (_registry != null)
			{
				_registry.Destroy(Entity);
			}
		}
	}
}
