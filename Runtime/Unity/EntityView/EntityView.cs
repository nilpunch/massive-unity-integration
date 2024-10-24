using System.Collections.Generic;
using UnityEngine;

namespace Massive.Unity
{
	public class EntityView : ComponentsView
	{
		[SerializeField] private List<MonoBehaviour> _viewBehaviours = new List<MonoBehaviour>();

		private readonly List<IViewBehaviour> _cachedViewBehaviours = new List<IViewBehaviour>();

		private void Awake()
		{
			gameObject.SetActive(false);

			foreach (MonoBehaviour monoBehaviour in _viewBehaviours)
			{
				if (monoBehaviour is IViewBehaviour viewBehaviour)
				{
					_cachedViewBehaviours.Add(viewBehaviour);
				}
			}
		}

		public void AssignEntity(Registry registry, Entity entity)
		{
			Entity = entity;
			Registry = registry;
			gameObject.SetActive(true);

			foreach (IViewBehaviour viewBehaviour in _cachedViewBehaviours)
			{
				viewBehaviour.OnEntityAssigned(registry, entity);
			}
		}

		public void UnassignEntity()
		{
			foreach (IViewBehaviour viewBehaviour in _cachedViewBehaviours)
			{
				viewBehaviour.OnEntityUnassigned();
			}

			gameObject.SetActive(false);
		}

#if UNITY_EDITOR
		[ContextMenu("Find behaviours")]
		private void FindBehaviours()
		{
			UnityEditor.Undo.RecordObject(this, "Find behaviours");
			IViewBehaviour[] behaviours = GetComponentsInChildren<IViewBehaviour>(true);

			_viewBehaviours.Clear();
			foreach (IViewBehaviour behaviour in behaviours)
			{
				if (behaviour is MonoBehaviour monoBehaviour)
				{
					_viewBehaviours.Add(monoBehaviour);
				}
			}

			UnityEditor.EditorUtility.SetDirty(this);
		}
#endif
	}
}
