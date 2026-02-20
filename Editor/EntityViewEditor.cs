#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Massive.Unity.Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(EntityView))]
	internal class EntityViewEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			if (!DrawRegisterViewButton())
			{
				EditorGUILayout.Space();
			}
			DrawCollectViewBehavioursButton();
			EditorGUILayout.Space();
			DrawDefaultInspector();
		}

		private void DrawCollectViewBehavioursButton()
		{
			if (GUILayout.Button("Collect View Behaviours", GUILayout.Height(25)))
			{
				Undo.RecordObjects(targets, "Collect View Behaviours");

				foreach (var obj in targets)
				{
					var entityView = obj as EntityView;
					if (entityView == null)
					{
						continue;
					}

					entityView.CollectViewBehaviours();
					EditorUtility.SetDirty(entityView);
				}
			}
		}

		private bool DrawRegisterViewButton()
		{
			if (ViewDataBase.Instance == null)
			{
				return false;
			}

			var unregisteredCount = 0;

			foreach (var obj in targets)
			{
				var entityView = obj as EntityView;
				if (entityView == null)
				{
					continue;
				}

				if (IsPrefabForRegistration(entityView, out var prefabAsset)
					&& !ViewDataBase.Instance.IsRegistered(prefabAsset))
				{
					unregisteredCount++;
				}
			}

			if (unregisteredCount > 0)
			{
				EditorGUILayout.Space();

				var buttonText = targets.Length == 1
					? "Register View"
					: $"Register {unregisteredCount} Views";

				if (GUILayout.Button(buttonText, GUILayout.Height(25)))
				{
					Undo.RecordObject(ViewDataBase.Instance, "Register View Prefabs");

					foreach (var obj in targets)
					{
						var entityView = obj as EntityView;
						if (entityView == null)
						{
							continue;
						}

						if (IsPrefabForRegistration(entityView, out var prefabAsset)
							&& !ViewDataBase.Instance.IsRegistered(prefabAsset))
						{
							ViewDataBase.Instance.Register(prefabAsset);
						}
					}

					EditorUtility.SetDirty(ViewDataBase.Instance);
				}

				EditorGUILayout.Space();

				return true;
			}

			return false;
		}

		private bool IsPrefabForRegistration(EntityView entityView, out EntityView prefabAsset)
		{
			prefabAsset = null;

			if (PrefabUtility.IsPartOfPrefabAsset(entityView))
			{
				prefabAsset = entityView;
				return true;
			}

			var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
			if (prefabStage != null && prefabStage.IsPartOfPrefabContents(entityView.gameObject))
			{
				var assetPath = prefabStage.assetPath;
				if (!string.IsNullOrEmpty(assetPath))
				{
					prefabAsset = AssetDatabase.LoadAssetAtPath<EntityView>(assetPath);
					return prefabAsset != null;
				}
			}

			if (PrefabUtility.IsPartOfAnyPrefab(entityView))
			{
				prefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(entityView);
				return prefabAsset != null;
			}

			return false;
		}
	}
}
#endif
