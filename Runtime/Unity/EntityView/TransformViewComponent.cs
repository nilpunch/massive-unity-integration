using UnityEngine;

namespace Massive.Unity
{
	public class TransformViewComponent : ViewComponent
	{
		[SerializeField] private Transform _rootTransform;

		public override void Register(Registry registry, Entity viewEntity)
		{
			registry.Assign(viewEntity, _rootTransform);
		}
	}
}
