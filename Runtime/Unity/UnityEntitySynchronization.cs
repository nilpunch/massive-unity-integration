using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Massive.Unity
{
	public class UnityEntitySynchronization : IComponentsEventHandler, IDisposable
	{
		private readonly IRegistry _registry;
		private readonly Dictionary<Entity, GameObject> _gameObjects;
		private int _entityCounter;

		public UnityEntitySynchronization(IRegistry registry)
		{
			_registry = registry;
			_gameObjects = new Dictionary<Entity, GameObject>();

			for (var i = 0; i < _registry.Entities.Alive.Length; i++)
			{
				OnAfterEntityCreated(_registry.Entities.Alive[i]);
			}

			_registry.Entities.AfterCreated += OnAfterEntityCreated;
			_registry.Entities.BeforeDestroyed += OnBeforeEntityDestroyed;

			foreach (var reflector in ComponentReflectors.All)
			{
				reflector.SynchronizeGameObjects(_registry, this);
			}

			foreach (var reflector in ComponentReflectors.All)
			{
				reflector.SubscribeAssignCallbacks(_registry, this);
			}
		}

		private void OnAfterEntityCreated(Entity entity)
		{
			var go = new GameObject($"Entity {++_entityCounter}");
			go.AddComponent<MonoEntity>().Synchronize(entity);
			_gameObjects.Add(entity, go);
		}

		private void OnBeforeEntityDestroyed(int entityId)
		{
			var entity = _registry.GetEntity(entityId);
			if (_gameObjects.TryGetValue(entity, out var go))
			{
				Object.Destroy(go);
				_gameObjects.Remove(entity);
			}
		}

		public void OnAfterAssigned<TMonoComponent>(int entityId) where TMonoComponent : MonoComponent
		{
			var entity = _registry.GetEntity(entityId);
			if (_gameObjects.TryGetValue(entity, out var go))
			{
				go.AddComponent<TMonoComponent>().Synchronize(_registry, entity);
			}
		}

		public void OnBeforeUnassigned<TMonoComponent>(int entityId) where TMonoComponent : MonoComponent
		{
			var entity = _registry.GetEntity(entityId);
			if (_gameObjects.TryGetValue(entity, out var go))
			{
				Object.Destroy(go.GetComponent<TMonoComponent>());
			}
		}

		public void Dispose()
		{
			_registry.Entities.AfterCreated -= OnAfterEntityCreated;
			_registry.Entities.BeforeDestroyed -= OnBeforeEntityDestroyed;

			foreach (var reflector in ComponentReflectors.All)
			{
				reflector.UnsubscribeAssignCallbacks(_registry, this);
			}

			foreach (var go in _gameObjects.Values)
			{
				Object.Destroy(go);
			}

			_gameObjects.Clear();
		}
	}
}
