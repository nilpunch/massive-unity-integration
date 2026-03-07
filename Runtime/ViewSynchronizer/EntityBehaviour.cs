using UnityEngine;

namespace Massive.Unity
{
	public abstract class EntityBehaviour : MonoBehaviour
	{
		public Entity Entity { get; private set; }

		public void AssignEntity(Entity entity)
		{
			Entity = entity;
			OnEntityAssigned();
		}

		public void RemoveEntity()
		{
			OnEntityRemoved();
			Entity = Entity.Dead;
		}

		protected virtual void OnEntityAssigned() { }
		protected virtual void OnEntityRemoved() { }
	}
}
