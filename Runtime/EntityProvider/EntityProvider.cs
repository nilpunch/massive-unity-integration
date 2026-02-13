using System.Collections.Generic;
using Massive.QoL;
using UnityEngine;

namespace Massive.Unity
{
	public enum EntityCreation
	{
		Automatic,
		Manually,
	}

	public enum CreatedAction
	{
		Persist,
		DestroyUnityComponent,
		DestroyGameObject,
	}

	[DisallowMultipleComponent]
	public class EntityProvider : MonoBehaviour
	{
		[SerializeField, StaticWorldSelector] private string _world;

		[SerializeReference, ComponentSelector]
		private List<object> _components = new List<object>();

		[SerializeField] private EntityCreation _create;
		[SerializeField] private CreatedAction _whenCreated;

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

		public List<object> Components => _components;

		protected virtual void Reset()
		{
			_world = StaticWorlds.WorldNames.Length == 0 ? null : StaticWorlds.WorldNames[0];
		}

		protected virtual void Awake()
		{
			if (_create == EntityCreation.Automatic)
			{
				Entity = World.CreateEntity();
			}
		}

		protected virtual void Start()
		{
			if (_create == EntityCreation.Automatic)
			{
				SetComponents(Entity);
				TriggerOnCreate();
			}
		}

		public void SetComponents(Entity entity)
		{
			foreach (var component in _components)
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
			switch (_whenCreated)
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
