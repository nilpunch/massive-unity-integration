namespace UPR
{
	public interface IComponentsEventHandler
	{
		void OnAfterAssigned<TMonoComponent>(int entityId)
			where TMonoComponent : MonoComponent;

		void OnBeforeUnassigned<TMonoComponent>(int entityId)
			where TMonoComponent : MonoComponent;
	}
}
