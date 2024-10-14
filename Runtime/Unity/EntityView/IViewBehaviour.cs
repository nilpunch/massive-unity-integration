namespace Massive.Unity
{
	public interface IViewBehaviour
	{
		void OnEntityAssigned(Registry registry, Entity entity);

		void OnEntityUnassigned();
	}
}
