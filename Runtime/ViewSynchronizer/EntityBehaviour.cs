using UnityEngine;

namespace Massive.Unity
{
	public abstract class EntityBehaviour : MonoBehaviour
	{
		public abstract void OnEntityAssigned(Entity entity);
		public abstract void OnEntityRemoved();
	}
}
