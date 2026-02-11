using System.Collections.Generic;
using Massive.QoL;
using UnityEngine;

namespace Massive.Unity
{
	public class EntityView : MonoBehaviour, IEntityView
	{
		[SerializeField] private List<EntityBehaviour> _entityBehaviours = new List<EntityBehaviour>();

		public Entity Entity { get; protected set; }

		public void AssignEntity(Entity entity)
		{
			Entity = entity;

			gameObject.SetActive(true);

			foreach (var viewBehaviour in _entityBehaviours)
			{
				viewBehaviour.OnEntityAssigned(Entity);
			}
		}

		public void RemoveEntity()
		{
			foreach (var viewBehaviour in _entityBehaviours)
			{
				viewBehaviour.OnEntityRemoved();
			}

			gameObject.SetActive(false);

			Entity = Entity.Dead;
		}

#if UNITY_EDITOR
		[ContextMenu("Find Behaviours and Components")]
		public void CollectViewBehaviours()
		{
			UnityEditor.Undo.RecordObject(this, "Find behaviours");
			var behaviours = GetComponentsInChildren<EntityBehaviour>(true);

			_entityBehaviours.Clear();
			foreach (var behaviour in behaviours)
			{
				_entityBehaviours.Add(behaviour);
			}

			UnityEditor.EditorUtility.SetDirty(this);
		}
#endif
	}
}
