using UnityEngine;

namespace Massive.Unity
{
	[DisallowMultipleComponent]
	public class MonoEntity : MonoBehaviour
	{
		private IRegistry _registry;

		public Entity Entity { get; private set; }

		public void ApplyToRegistry(IRegistry registry)
		{
			var entity = registry.CreateEntity();

			foreach (var monoComponent in GetComponents<MonoComponent>())
			{
				monoComponent.ApplyToEntity(registry, entity);
			}
		}

		public void Synchronize(IRegistry registry, Entity entity)
		{
			_registry = registry;
			Entity = entity;
		}

		[ContextMenu("Destroy Entity")]
		private void DestroyEntity()
		{
			if (_registry != null)
			{
				_registry.Destroy(Entity);
			}
		}
	}
}
