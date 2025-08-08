namespace Massive.Unity.Samples.Farm
{
	public class GizmosFeature : Feature, IDrawGizmos
	{
		private readonly FastList<IDrawGizmos> _drawGizmoSystems = new FastList<IDrawGizmos>();

		public GizmosFeature(World world) : base(world)
		{
		}

		protected override void OnSystemAdded(ISystem system)
		{
			if (system is IDrawGizmos drawGizmos)
			{
				_drawGizmoSystems.Add(drawGizmos);
			}
		}

		public void OnDrawGizmos()
		{
			foreach (var drawGizmoSystem in _drawGizmoSystems)
			{
				drawGizmoSystem.OnDrawGizmos();
			}
		}
	}

	public interface IDrawGizmos
	{
		void OnDrawGizmos();
	}
}
