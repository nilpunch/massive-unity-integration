using System.Collections.Generic;
using Massive.QoL;
using UnityEngine;

namespace Massive.Unity
{
	[DisallowMultipleComponent]
	public abstract class EntityProviderBase : MonoBehaviour
	{
		[SerializeField, StaticWorldSelector] private string _world;

		private World _cachedWorld;

		public World World
		{
			get
			{
				if (_cachedWorld == null)
				{
					StaticWorlds.TryGetWorldByName(_world, out _cachedWorld);
				}

				return _cachedWorld;
			}
		}

		public Entity Entity { get; private set; }

		public abstract List<object> Components { get; }
		public abstract EntityCreation EntityCreation { get; }
		public abstract CreatedAction CreatedAction { get; }

		protected virtual void Reset()
		{
			_world = StaticWorlds.WorldNames.Length == 0 ? null : StaticWorlds.WorldNames[0];
		}

		protected virtual void Awake()
		{
			if (EntityCreation == EntityCreation.Automatically)
			{
				Entity = World.CreateEntity();
			}
		}

		protected virtual void Start()
		{
			if (EntityCreation == EntityCreation.Automatically)
			{
				SetComponents(Entity);
				TriggerOnCreate();
			}
		}

		public void SetComponents(Entity entity)
		{
			foreach (var component in Components)
			{
				if (component is IComponentConverter componentProvider)
				{
					componentProvider.ApplyComponent(entity, transform);
					continue;
				}

				var set = World.Sets.GetReflected(component.GetType());
				set.Add(entity.Id);
				if (set is IDataSet dataSet)
				{
					dataSet.SetRaw(entity.Id, component);
				}
			}
		}

		public void TriggerOnCreate()
		{
			switch (CreatedAction)
			{
				case CreatedAction.DestroyUnityComponent:
					Destroy(this);
					return;
				case CreatedAction.DestroyGameObject:
					Destroy(gameObject);
					return;
			}
		}
	}
}
