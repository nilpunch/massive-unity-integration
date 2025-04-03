using UnityEngine;

namespace Massive.Unity
{
	[RequireComponent(typeof(EntityProvider))]
	[DisallowMultipleComponent]
	public class TagProvider<TComponent> : ComponentProvider
	{
		private World _world;
		private Entity _entity;

		public override void ApplyToEntity(ServiceLocator serviceLocator, Entity entity)
		{
			serviceLocator.Find<World>().Add<TComponent>(entity);
		}
	}
}
