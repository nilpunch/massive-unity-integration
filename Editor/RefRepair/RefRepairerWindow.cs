#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Massive.Unity.Editor
{
	public class RefRepairerWindow : EditorWindow
	{
		public const string Title = "Reference Repairer";

		private MissingRefContainer _missingRefContainer = new MissingRefContainer();
		private MissingsResolvingData[] _cachedMissingsResolvingDatas = Array.Empty<MissingsResolvingData>();
		private Action<Type>[] _missingTypeSelectors = Array.Empty<Action<Type>>();
		private static bool _isNoFound;
		private Vector2 _scrollPosition;

		[MenuItem("Window/" + Constants.LibraryName + "/" + Title)]
		public static void Open()
		{
			var wnd = GetWindow<RefRepairerWindow>();
			wnd.titleContent = new GUIContent(Title);
			wnd.minSize = new Vector2(260f, 160f);
			wnd.Show();
		}

		private void OnGUI()
		{
			if (_missingRefContainer.IsEmpty)
			{
				if (GUILayout.Button("Collect Missings", GUI.skin.button))
				{
					_isNoFound = false;
					if (TryInitAndCollect())
					{
						_cachedMissingsResolvingDatas = _missingRefContainer.MissingsResolvingDatas.Values.ToArray();
						InitTypeSelectors();
						if (_missingRefContainer.IsEmpty)
						{
							_isNoFound = true;
						}
					}
				}

				if (_isNoFound)
				{
					GUILayout.Label("Missing references not found in the project.");
				}

				return;
			}

			if (_missingRefContainer.MissingsResolvingDatas.Count != _cachedMissingsResolvingDatas.Length)
			{
				_cachedMissingsResolvingDatas = _missingRefContainer.MissingsResolvingDatas.Values.ToArray();
				InitTypeSelectors();
			}

			GUILayout.Label("Missings to resolve:");
			
			GUILayout.Space(5f);

			// Add scrolling view here
			_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

			for (var i = 0; i < _cachedMissingsResolvingDatas.Length; i++)
			{
				var missing = _cachedMissingsResolvingDatas[i];
				if (i != 0)
				{
					GUILayout.Space(5f);
				}

				GUILayout.Label($"{missing.OldTypeData.NamespaceName}.{missing.OldTypeData.ClassName} =>");

				var typeSelectRect = EditorGUILayout.GetControlRect();
				SerializeReferenceGui.DrawTypeSelector(typeSelectRect, _cachedMissingsResolvingDatas[i].FindNewType(), _missingTypeSelectors[i], MatchAny);

				bool MatchAny(Type type) => true;
			}

			EditorGUILayout.EndScrollView(); // End scrolling view

			GUILayout.Space(5f);

			if (GUILayout.Button("Repaire missing references"))
			{
				RepaireFileUtility.RepairAsset(_missingRefContainer);
				if (_missingRefContainer.IsEmpty)
				{
					_isNoFound = true;
				}
			}
		}

		private void InitTypeSelectors()
		{
			_missingTypeSelectors = new Action<Type>[_cachedMissingsResolvingDatas.Length];
			for (int i = 0; i < _missingTypeSelectors.Length; i++)
			{
				var resolvingData = _cachedMissingsResolvingDatas[i];
				_missingTypeSelectors[i] = selectedType =>
				{
					if (selectedType != null)
					{
						resolvingData.NewTypeData = new TypeData(selectedType.Name, selectedType.Namespace ?? "", selectedType.Assembly.GetName().Name);
					}
				};
			}
		}

		private bool TryInitAndCollect()
		{
			var allCurrentDirtyScenes = EditorSceneManager
				.GetSceneManagerSetup()
				.Where(sceneSetup => sceneSetup.isLoaded)
				.Select(sceneSetup => EditorSceneManager.GetSceneByPath(sceneSetup.path))
				.Where(scene => scene.isDirty)
				.ToArray();

			if (allCurrentDirtyScenes.Length != 0)
			{
				bool result = EditorUtility.DisplayDialog(
					"Current active scene(s) is dirty",
					"Please save all active scenes as they may be overwritten",
					"Save active scene and Continue",
					"Cancel update"
				);
				if (result == false)
					return false;

				foreach (var dirtyScene in allCurrentDirtyScenes)
					EditorSceneManager.SaveScene(dirtyScene);
			}

			_missingRefContainer.Collect();
			return true;
		}
	}
}
#endif