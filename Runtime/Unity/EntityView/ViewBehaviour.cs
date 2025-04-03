using UnityEngine;

namespace Massive.Unity
{
	public abstract class ViewBehaviour : MonoBehaviour
	{
		public abstract void OnEntityAssigned(World world, Entity entity);
		public abstract void OnEntityRemoved();
	}
}
