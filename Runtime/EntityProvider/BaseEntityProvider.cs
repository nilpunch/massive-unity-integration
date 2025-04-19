using UnityEngine;

namespace Massive.Unity
{
	[DisallowMultipleComponent]
	public class BaseEntityProvider : MonoBehaviour
	{
		[SerializeField, WorldSelector] private string _world;

		private World _cachedWorld;

		public World World
		{
			get
			{
				if (_cachedWorld == null)
				{
					Worlds.TryGetWorldByName(_world, out _cachedWorld);
				}

				return _cachedWorld;
			}
		}

		protected virtual void Reset()
		{
			if (Worlds.AllWorlds.Length == 0)
			{
				_world = null;
			}
			else
			{
				Worlds.TryGetWorldName(Worlds.AllWorlds[0], out _world);
			}
		}
	}
}
