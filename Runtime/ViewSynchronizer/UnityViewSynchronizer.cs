using Massive.QoL;

namespace Massive.Unity
{
	public class UnityViewSynchronizer : ViewSynchronizer<EntityView>
	{
		public UnityViewSynchronizer() : this(new EntityViewFactory())
		{
		}

		public UnityViewSynchronizer(IViewFactory<EntityView> viewFactory) : base(viewFactory)
		{
		}
	}
}
