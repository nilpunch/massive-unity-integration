using UnityEngine;

namespace Massive.Unity
{
	public abstract class ViewComponent : MonoBehaviour
	{
		public abstract void Register(World world, Entity viewEntity);
	}

	public abstract class ViewComponent<T> : ViewComponent where T : ViewComponent<T>
	{
		public override void Register(World world, Entity viewEntity)
		{
			world.Set(viewEntity, (T)this);
		}
	}
}
