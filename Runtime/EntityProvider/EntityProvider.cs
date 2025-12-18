using System;
using System.Collections.Generic;
using Massive.QoL;
using UnityEngine;

namespace Massive.Unity
{
	public enum EntityCreation
	{
		OnStart,
		OnAwake,
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

		protected virtual void Reset()
		{
			_world = StaticWorlds.WorldNames.Length == 0 ? null : StaticWorlds.WorldNames[0];
		}

		protected virtual void Awake()
		{
			if (_create == EntityCreation.OnAwake)
			{
				CreateEntity();
			}
		}

		protected virtual void Start()
		{
			if (_create == EntityCreation.OnStart)
			{
				CreateEntity();
			}
		}

		public void CreateEntity()
		{
			Entity = World.CreateEntity();

			foreach (var component in _components)
			{
				var sparseSet = World.Sets.GetReflected(component.GetType());
				sparseSet.Add(Entity.Id);
				if (sparseSet is IDataSet dataSet)
				{
					dataSet.SetRaw(Entity.Id, component);
				}
			}

			OnCreate();
		}

		private void OnCreate()
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
