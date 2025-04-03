using UnityEngine;

namespace Massive.Unity
{
	public class TransformViewComponent : ViewComponent
	{
		[SerializeField] private Transform _rootTransform;

		public override void Register(World world, Entity viewEntity)
		{
			world.Set(viewEntity, _rootTransform);
		}
	}
}
