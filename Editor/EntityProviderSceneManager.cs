#if UNITY_EDITOR
using System.Collections.Generic;
using Massive.QoL;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Massive.Unity.Editor
{
	[InitializeOnLoad]
	internal static class EntityProviderPreviewSystem
	{
		private static readonly Dictionary<EntityProvider, EntityView> _previews = new();
		private static readonly Dictionary<EntityProvider, ViewAsset> _previewAssets = new();
		private static readonly HashSet<EntityProvider> _trackedProviders = new();
		private static readonly Dictionary<EntityView, EntityProvider> _reverseLookup = new();

		static EntityProviderPreviewSystem()
		{
			AssemblyReloadEvents.beforeAssemblyReload += ClearAll;

			Selection.selectionChanged += OnSelectionChanged;
			SceneView.duringSceneGui += Update;
			Undo.undoRedoPerformed += SyncProviders;
			EditorApplication.hierarchyChanged += OnHierarchyChanged;
			PrefabStage.prefabStageOpened += _ => RefreshAll();
			PrefabStage.prefabStageClosing += _ => ClearAll();

			RefreshAll();
		}

		private static bool _isRedirectingSelection;
		private static ViewDataBase _viewDataBase;

		private static void OnSelectionChanged()
		{
			if (_isRedirectingSelection)
			{
				return;
			}

			var active = Selection.activeGameObject;
			if (active == null)
			{
				return;
			}

			var view = active.GetComponentInParent<EntityView>();
			if (view == null)
			{
				return;
			}

			if (!_reverseLookup.TryGetValue(view, out var provider))
			{
				return;
			}

			_isRedirectingSelection = true;

			EditorApplication.delayCall += OnDelayCall;

			void OnDelayCall()
			{
				Selection.activeObject = provider.gameObject;
				_isRedirectingSelection = false;

				EditorApplication.delayCall -= OnDelayCall;
			}
		}

		private static void Update(SceneView _)
		{
			if (Application.isPlaying || ViewDataBase.Instance == null)
			{
				ClearAll();
				_trackedProviders.Clear();
				return;
			}

			if (_viewDataBase != ViewDataBase.Instance)
			{
				RefreshAll();
			}

			_viewDataBase = ViewDataBase.Instance;

			foreach (var provider in _trackedProviders)
			{
				if (provider == null)
				{
					continue;
				}

				var currentAsset = GetViewAsset(provider);

				_previews.TryGetValue(provider, out var preview);
				_previewAssets.TryGetValue(provider, out var cachedAsset);

				if (currentAsset.Id < 0)
				{
					if (preview != null)
					{
						RemovePreview(provider);
					}

					continue;
				}

				if (preview == null)
				{
					CreatePreview(provider, currentAsset);
					continue;
				}

				var prefab = ViewDataBase.Instance.GetViewPrefabOrNull(currentAsset);

				if (prefab == null)
				{
					if (preview != null)
					{
						RemovePreview(provider);
					}

					continue;
				}

				if (preview == null)
				{
					CreatePreview(provider, currentAsset);
					continue;
				}

				if (!Equals(currentAsset, cachedAsset))
				{
					RemovePreview(provider);
					CreatePreview(provider, currentAsset);
					continue;
				}

				preview.transform.SetPositionAndRotation(provider.transform.position, provider.transform.rotation);
			}
		}

		private static void OnHierarchyChanged()
		{
			if (Application.isPlaying || ViewDataBase.Instance == null)
			{
				ClearAll();
				return;
			}

			SyncProviders();
		}

		private static void RefreshAll()
		{
			ClearAll();
			SyncProviders();
		}

		private static void SyncProviders()
		{
			var providers = GetAllProviders();

			var toRemove = new List<EntityProvider>();

			foreach (var existing in _trackedProviders)
			{
				if (!providers.Contains(existing) || existing == null)
				{
					toRemove.Add(existing);
				}
			}

			foreach (var remove in toRemove)
			{
				RemovePreview(remove);
				_trackedProviders.Remove(remove);
			}

			foreach (var provider in providers)
			{
				_trackedProviders.Add(provider);
			}
		}

		private static HashSet<EntityProvider> GetAllProviders()
		{
			var result = new HashSet<EntityProvider>();

			for (var i = 0; i < SceneManager.sceneCount; i++)
			{
				var scene = SceneManager.GetSceneAt(i);
				if (!scene.isLoaded)
				{
					continue;
				}

				foreach (var root in scene.GetRootGameObjects())
				{
					var providers = root.GetComponentsInChildren<EntityProvider>(true);
					foreach (var provider in providers)
					{
						result.Add(provider);
					}
				}
			}

			var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
			if (prefabStage != null)
			{
				var providers = prefabStage.prefabContentsRoot.GetComponentsInChildren<EntityProvider>(true);

				foreach (var provider in providers)
				{
					result.Add(provider);
				}
			}

			return result;
		}

		private static void CreatePreview(EntityProvider provider, ViewAsset viewAsset)
		{
			if (provider == null)
			{
				return;
			}

			var prefab = ViewDataBase.Instance.GetViewPrefabOrNull(viewAsset);
			if (prefab == null)
			{
				return;
			}

			var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
			var parent = prefabStage != null ? prefabStage.prefabContentsRoot.transform : null;

			var preview = (EntityView)PrefabUtility.InstantiatePrefab(prefab, parent);

			preview.gameObject.tag = "EditorOnly";
			preview.gameObject.hideFlags = HideFlags.HideAndDontSave | HideFlags.NotEditable;

			_previews[provider] = preview;
			_previewAssets[provider] = viewAsset;
			_reverseLookup[preview] = provider;
		}

		private static void RemovePreview(EntityProvider provider)
		{
			if (_previews.TryGetValue(provider, out var preview) && preview != null)
			{
				_reverseLookup.Remove(preview);
				Object.DestroyImmediate(preview.gameObject);
			}

			_previews.Remove(provider);
			_previewAssets.Remove(provider);
		}

		private static void ClearAll()
		{
			foreach (var preview in _previews.Values)
			{
				if (preview != null)
				{
					Object.DestroyImmediate(preview.gameObject);
				}
			}

			_previews.Clear();
			_previewAssets.Clear();
		}

		private static ViewAsset GetViewAsset(EntityProvider entityProvider)
		{
			foreach (var component in entityProvider.Components)
			{
				if (component is ViewAsset viewAsset)
				{
					return viewAsset;
				}
			}

			return default;
		}
	}
}
#endif
