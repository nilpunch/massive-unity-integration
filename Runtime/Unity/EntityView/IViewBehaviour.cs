namespace Massive.Unity
{
	public interface IViewBehaviour
	{
		void OnEntityAssigned(IRegistry registry, Entity entity);

		void OnEntityUnassigned();
	}
}