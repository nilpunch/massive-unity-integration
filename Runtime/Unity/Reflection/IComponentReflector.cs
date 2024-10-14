using Massive.Serialization;

namespace Massive.Unity
{
	public interface IComponentReflector
	{
		void SynchronizeComponents(Registry registry, DataSet<MonoEntity> monoEntities, IComponentsEventHandler componentsEventHandler);
		void PopulateRegistryParser(RegistrySerializer registrySerializer);
		void SubscribeAssignCallbacks(Registry registry, IComponentsEventHandler eventHandler);
		void UnsubscribeAssignCallbacks(Registry registry, IComponentsEventHandler eventHandler);
	}
}
