using Massive.QoL;
using UnityEngine;

namespace Massive.Unity
{
	[DisallowMultipleComponent]
	public abstract class BaseEntityProvider : MonoBehaviour
	{
		[SerializeField, StaticWorldSelector] private string _world;

		private World _cachedWorld;

		public World World
		{
			get
			{
				if (_cachedWorld == null)
				{
					StaticWorlds.TryGetWorldByName(_world, out _cachedWorld);
				}

				return _cachedWorld;
			}
		}

		protected virtual void Reset()
		{
			_world = StaticWorlds.WorldNames.Length == 0 ? null : StaticWorlds.WorldNames[0];
		}
	}
}
