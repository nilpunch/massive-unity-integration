﻿using Massive.Serialization;

namespace Massive.Unity
{
	public class UnmanagedComponentReflector<TComponent, TMonoComponent> : IComponentReflector
		where TComponent : unmanaged
		where TMonoComponent : UnmanagedComponentBase<TComponent, TMonoComponent>
	{
		public void SynchronizeComponents(IRegistry registry, IComponentsEventHandler componentsEventHandler)
		{
			var components = registry.Any<TComponent>();
			foreach (var entity in registry.Entities.Alive)
			{
				if (components.IsAssigned(entity.Id))
				{
					componentsEventHandler.OnAfterAssigned<TMonoComponent>(entity.Id);
				}
				else
				{
					componentsEventHandler.OnBeforeUnassigned<TMonoComponent>(entity.Id);
				}
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
