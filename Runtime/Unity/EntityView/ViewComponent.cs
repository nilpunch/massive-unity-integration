using UnityEngine;

namespace Massive.Unity
{
	public abstract class ViewComponent : MonoBehaviour
	{
		public abstract void Register(Registry registry, Entity viewEntity);
	}

	public abstract class ViewComponent<T> : ViewComponent where T : ViewComponent<T>
	{
		public override void Register(Registry registry, Entity viewEntity)
		{
			registry.Assign(viewEntity, (T)this);
		}
	}
}
