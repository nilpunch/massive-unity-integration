using Massive.Serialization;

namespace Massive.Unity
{
	public class UnmanagedComponentReflector<TComponent, TMonoComponent> : IComponentReflector
		where TComponent : unmanaged
		where TMonoComponent : UnmanagedComponentBase<TComponent, TMonoComponent>
	{
		public void SynchronizeComponents(IRegistry registry, IReadOnlyDataSet<MonoEntity> monoEntities, IComponentsEventHandler componentsEventHandler)
		{
			var components = registry.Any<TComponent>();
			var componentsIds = components.Ids;

			var monoEntityIds = monoEntities.Ids;
			var monoEntityData = monoEntities.Data;
			for (int i = 0; i < monoEntityIds.Length; i++)
			{
				int entityId = monoEntityIds[i];
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
