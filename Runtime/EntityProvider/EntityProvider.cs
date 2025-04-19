using System;
using UnityEngine;

namespace Massive.Unity
{
	public class EntityProvider : MonoBehaviour
	{
		[SerializeField, WorldName] private string _worldTypeName;

		private World _world;

		public World World
		{
			get
			{
				if (_world == null)
				{
					Worlds.TryGetWorldByName(_worldTypeName, out _world);
				}

				return _world;
			}
		}

		private void Reset()
		{
			if (Worlds.AllWorlds.Length == 0)
			{
				_worldTypeName = null;
			}
			else
			{
				Worlds.TryGetWorldName(Worlds.AllWorlds[0], out _worldTypeName);
			}
		}
	}
}
