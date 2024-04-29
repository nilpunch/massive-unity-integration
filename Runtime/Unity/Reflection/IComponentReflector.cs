using Massive;
using Massive.Serialization;

namespace UPR
{
	public interface IComponentReflector
	{
		void SynchronizeGameObjects(IRegistry registry, IComponentsEventHandler componentsEventHandler);
		void PopulateRegistryParser(RegistryParser registryParser);
		void SubscribeAssignCallbacks(IRegistry registry, IComponentsEventHandler eventHandler);
		void UnsubscribeAssignCallbacks(IRegistry registry, IComponentsEventHandler eventHandler);
	}
}
