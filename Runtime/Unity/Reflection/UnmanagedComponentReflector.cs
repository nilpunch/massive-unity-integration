﻿using Massive;
using Massive.Serialization;

namespace UPR
{
	public class UnmanagedComponentReflector<TComponent, TMonoComponent> : IComponentReflector
		where TComponent : unmanaged
		where TMonoComponent : UnmanagedComponentBase<TComponent, TMonoComponent>
	{
		public void SynchronizeGameObjects(IRegistry registry, IComponentsEventHandler componentsEventHandler)
		{
			foreach (var id in registry.Any<TComponent>().Ids)
			{
				componentsEventHandler.OnAfterAssigned<TMonoComponent>(id);
			}
		}

		public void PopulateRegistryParser(RegistryParser registryParser)
		{
			registryParser.AddComponent<TComponent>();
		}

		public void SubscribeAssignCallbacks(IRegistry registry, IComponentsEventHandler eventHandler)
		{
			registry.Any<TComponent>().AfterAssigned += eventHandler.OnAfterAssigned<TMonoComponent>;
			registry.Any<TComponent>().BeforeUnassigned += eventHandler.OnBeforeUnassigned<TMonoComponent>;
		}

		public void UnsubscribeAssignCallbacks(IRegistry registry, IComponentsEventHandler eventHandler)
		{
			registry.Any<TComponent>().AfterAssigned -= eventHandler.OnAfterAssigned<TMonoComponent>;
			registry.Any<TComponent>().BeforeUnassigned -= eventHandler.OnBeforeUnassigned<TMonoComponent>;
		}
	}
}