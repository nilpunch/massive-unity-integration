#if UNITY_EDITOR
using System.Linq;

namespace Massive.Unity.Editor
{
	internal static class EditorCache
	{
		private static string[] s_worldNames;
		private static string[] s_formatedWorldNames;

		public static string[] WorldNames
		{
			get
			{
				if (s_worldNames == null || s_worldNames.Length != Worlds.AllWorldsNames.Length)
				{
					s_worldNames = Worlds.AllWorldsNames.ToArray();
				}

				return s_worldNames;
			}
		}

		public static string[] FormatedWorldNames
		{
			get
			{
				if (s_formatedWorldNames == null || s_formatedWorldNames.Length != Worlds.AllWorldsNames.Length)
				{
					s_formatedWorldNames = Worlds.AllWorldsNames.ToArray();
					for (var i = 0; i < s_formatedWorldNames.Length; i++)
					{
						var name = s_formatedWorldNames[i];
						var lastDot = name.LastIndexOf('.');
						s_formatedWorldNames[i] = lastDot >= 0 ? $"{name.Substring(lastDot + 1)} ({name.Substring(0, lastDot)})" : name;
					}
				}

				return s_formatedWorldNames;
			}
		}
	}
}
#endif
