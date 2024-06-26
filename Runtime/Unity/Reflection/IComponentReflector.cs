﻿using Massive.Serialization;

namespace Massive.Unity
{
	public interface IComponentReflector
	{
		void SynchronizeComponents(IRegistry registry, IReadOnlyDataSet<MonoEntity> monoEntities, IComponentsEventHandler componentsEventHandler);
		void PopulateRegistryParser(RegistrySerializer registrySerializer);
		void SubscribeAssignCallbacks(IRegistry registry, IComponentsEventHandler eventHandler);
		void UnsubscribeAssignCallbacks(IRegistry registry, IComponentsEventHandler eventHandler);
	}
}
