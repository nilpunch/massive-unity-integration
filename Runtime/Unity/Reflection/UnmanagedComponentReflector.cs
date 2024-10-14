using Massive.Serialization;

namespace Massive.Unity
{
	public class UnmanagedComponentReflector<TComponent, TMonoComponent> : IComponentReflector
		where TComponent : unmanaged
		where TMonoComponent : UnmanagedComponentBase<TComponent, TMonoComponent>
	{
		public void SynchronizeComponents(Registry registry, DataSet<MonoEntity> monoEntities, IComponentsEventHandler componentsEventHandler)
		{
			var components = registry.Set<TComponent>();

			var monoEntityData = monoEntities.Data;
			for (int i = 0; i < monoEntities.Count; i++)
			{
				int entityId = monoEntities.Ids[i];
				var monoEntity = monoEntityData[i];
				if (!components.IsAssigned(entityId) || monoEntity.Entity != registry.GetEntity(entityId))
				{
					componentsEventHandler.OnBeforeUnassigned<TMonoComponent>(entityId);
				}
				else
				{
					componentsEventHandler.OnAfterAssigned<TMonoComponent>(entityId);
				}
			}
		}

		public void PopulateRegistryParser(RegistrySerializer registrySerializer)
		{
			registrySerializer.AddComponent<TComponent>();
		}

		public void SubscribeAssignCallbacks(Registry registry, IComponentsEventHandler eventHandler)
		{
			registry.Set<TComponent>().AfterAssigned += eventHandler.OnAfterAssigned<TMonoComponent>;
			registry.Set<TComponent>().BeforeUnassigned += eventHandler.OnBeforeUnassigned<TMonoComponent>;
		}

		public void UnsubscribeAssignCallbacks(Registry registry, IComponentsEventHandler eventHandler)
		{
			registry.Set<TComponent>().AfterAssigned -= eventHandler.OnAfterAssigned<TMonoComponent>;
			registry.Set<TComponent>().BeforeUnassigned -= eventHandler.OnBeforeUnassigned<TMonoComponent>;
		}
	}
}
