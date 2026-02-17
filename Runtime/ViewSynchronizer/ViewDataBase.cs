using System.Collections.Generic;
using Massive.QoL;
using UnityEngine;

namespace Massive.Unity
{
	[CreateAssetMenu(menuName = "Massive/View DB", fileName = "View DB")]
	public class ViewDataBase : ScriptableConfig<ViewDataBase>
	{
		[field: SerializeField] public List<EntityView> ViewPrefabs { get; private set; } = new List<EntityView>();

		public ViewAsset GetViewAsset(EntityView prefab)
		{
			return new ViewAsset(ViewPrefabs.IndexOf(prefab));
		}

		public EntityView GetViewPrefab(ViewAsset viewAsset)
		{
			return ViewPrefabs[viewAsset.Id];
		}

		public EntityView GetViewPrefabOrNull(ViewAsset viewAsset)
		{
			return !viewAsset.IsValid || viewAsset.Id >= ViewPrefabs.Count ? null : ViewPrefabs[viewAsset.Id];
		}

		public bool IsRegistered(ViewAsset viewAsset)
		{
			return viewAsset.IsValid && viewAsset.Id < ViewPrefabs.Count && ViewPrefabs[viewAsset.Id] != null;
		}

		public bool IsRegistered(EntityView prefab)
		{
			return prefab != null && ViewPrefabs.Contains(prefab);
		}

		public void Register(EntityView prefab)
		{
			if (Application.isPlaying)
			{
				Debug.LogError("[ViewDataBase] Cannot register prefabs at runtime. Register in Editor only.");
				return;
			}

			if (prefab == null)
			{
				Debug.LogError("[ViewDataBase] Cannot register null prefab.");
				return;
			}

#if UNITY_EDITOR
			if (!UnityEditor.PrefabUtility.IsPartOfPrefabAsset(prefab))
			{
				Debug.LogError($"[ViewDataBase] '{prefab.name}' is not a prefab asset. Only prefabs can be registered.", prefab);
				return;
			}
#endif

			for (var i = 0; i < ViewPrefabs.Count; i++)
			{
				if (ViewPrefabs[i] == prefab)
				{
					Debug.LogWarning($"[ViewDataBase] '{prefab.name}' is already registered.", prefab);
					return;
				}
			}

			var registered = false;
			for (var i = 0; i < ViewPrefabs.Count; i++)
			{
				if (ViewPrefabs[i] == null)
				{
					ViewPrefabs[i] = prefab;
					registered = true;
					break;
				}
			}

			if (!registered)
			{
				ViewPrefabs.Add(prefab);
			}

			Debug.Log($"[ViewDataBase] Registered '{prefab.name}'.", prefab);
		}
	}
}
