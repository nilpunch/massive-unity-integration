#if UNITY_EDITOR
using System.Linq;

namespace Massive.Unity.Editor
{
	internal static class EditorCache
	{
		private static string[] s_worldNames;

		public static string[] WorldNames
		{
			get
			{
				if (s_worldNames == null || s_worldNames.Length != Worlds.AllWorlds.Length)
				{
					s_worldNames = Worlds.AllWorlds.ToArray().Select(Worlds.GetWorldName).ToArray();
				}

				return s_worldNames;
			}
		}
	}
}
#endif
