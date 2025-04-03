using System.Collections.Generic;
using UnityEngine;

namespace Massive.Unity
{
	public class EntityView : MonoBehaviour
	{
		[SerializeField] private List<ViewBehaviour> _viewBehaviours = new List<ViewBehaviour>();
		[SerializeField] private List<ViewComponent> _viewComponents = new List<ViewComponent>();

		public World World { get; protected set; }

		public Entity Entity  { get; protected set; }

		public void Register(World world, Entity viewEntity)
		{
			world.Set(viewEntity, this);
			foreach (var viewComponent in _viewComponents)
			{
				viewComponent.Register(world, viewEntity);
			}
		}

		public void AssignEntity(World world, Entity entity)
		{
			Entity = entity;
			World = world;

			foreach (var viewBehaviour in _viewBehaviours)
			{
				viewBehaviour.OnEntityAssigned(world, entity);
			}
		}

		public void RemoveEntity()
		{
			foreach (var viewBehaviour in _viewBehaviours)
			{
				viewBehaviour.OnEntityRemoved();
			}

			gameObject.SetActive(false);
		}

#if UNITY_EDITOR
		[ContextMenu("Find Behaviours and Components")]
		private void FindBehavioursAndComponents()
		{
			UnityEditor.Undo.RecordObject(this, "Find behaviours");
			var behaviours = GetComponentsInChildren<ViewBehaviour>(true);
			var components = GetComponentsInChildren<ViewComponent>(true);

			_viewBehaviours.Clear();
			foreach (var behaviour in behaviours)
			{
				_viewBehaviours.Add(behaviour);
			}

			_viewComponents.Clear();
			foreach (var component in components)
			{
				_viewComponents.Add(component);
			}

			UnityEditor.EditorUtility.SetDirty(this);
		}
#endif
	}
}
