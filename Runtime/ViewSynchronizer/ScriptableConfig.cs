using System;
using System.Linq;
using UnityEngine;

namespace Massive.Unity
{
	public abstract class ScriptableConfig : ScriptableObject
	{
#if UNITY_EDITOR
		public abstract bool IsMainInstance { get; }

		public abstract void SetAsMainInstance();
#endif
	}

	// Credits go to this guy: https://github.com/blackbone.
	public class ScriptableConfig<T> : ScriptableConfig where T : ScriptableConfig<T>
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance != null)
				{
					return _instance;
				}

				if (Application.isPlaying)
				{
					LoadRuntime();
				}
#if UNITY_EDITOR
				else
				{
					LoadEditor();
				}
#endif
				return _instance;
			}
		}

		private static void LoadRuntime() => _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();

#if UNITY_EDITOR
		private void Awake()
		{
			if (Application.isPlaying)
			{
				return;
			}

			var preloads = UnityEditor.PlayerSettings.GetPreloadedAssets();
			if (preloads.Contains(this))
			{
				return;
			}

			if (preloads.Any(p => p is T))
			{
				return;
			}

			Array.Resize(ref preloads, preloads.Length + 1);
			preloads[^1] = this;
			UnityEditor.PlayerSettings.SetPreloadedAssets(preloads);
		}

		private static void LoadEditor()
		{
			if (Application.isPlaying)
			{
				return;
			}
			_instance = UnityEditor.PlayerSettings.GetPreloadedAssets().FirstOrDefault(p => p is T) as T;
		}

		public override bool IsMainInstance => Instance == this;

		public override void SetAsMainInstance()
		{
			if (Application.isPlaying)
			{
				return;
			}

			if (IsMainInstance)
			{
				return;
			}

			_instance = null;

			var preloads = UnityEditor.PlayerSettings.GetPreloadedAssets();
			preloads[Array.FindIndex(preloads, p => p is T || p == null)] = this;
			UnityEditor.PlayerSettings.SetPreloadedAssets(preloads);
		}
#endif
	}
}
