using UnityEngine;

namespace Massive.Unity
{
	public abstract class ViewBehaviour : MonoBehaviour
	{
		public abstract void OnEntityAssigned(Registry registry, Entity entity);
		public abstract void OnEntityUnassigned();
	}
}
