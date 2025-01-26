using UnityEngine;

namespace Massive.Unity
{
	[RequireComponent(typeof(EntityProvider))]
	public abstract class ComponentProvider : MonoBehaviour
	{
		public abstract void ApplyToEntity(ServiceLocator serviceLocator, Entity entity);
	}

	[RequireComponent(typeof(EntityProvider))]
	[DisallowMultipleComponent]
	public class ComponentProvider<TComponent> : ComponentProvider
	{
		[SerializeField, Unfold] private TComponent _data;

		public override void ApplyToEntity(ServiceLocator serviceLocator, Entity entity)
		{
			serviceLocator.Find<Registry>().Assign(entity, _data);
		}
	}

	[RequireComponent(typeof(EntityProvider))]
	[DisallowMultipleComponent]
	public class ComponentProviderInline<TComponent> : ComponentProvider
	{
		[SerializeField] private TComponent _data;

		public override void ApplyToEntity(ServiceLocator serviceLocator, Entity entity)
		{
			serviceLocator.Find<Registry>().Assign(entity, _data);
		}
	}
}
