using Massive.QoL;

namespace Massive.Unity
{
	public class UnityViewSynchronizer : ViewSynchronizer<EntityView>
	{
		public UnityViewSynchronizer(World world, IViewFactory<EntityView> viewFactory) : base(world, viewFactory)
		{
		}
	}
}
