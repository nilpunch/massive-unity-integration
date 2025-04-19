using System;
using Unity.IL2CPP.CompilerServices;

namespace Massive.Unity
{
	[Il2CppEagerStaticClassConstruction]
	public partial class World<TWorldType>
	{
		// ReSharper disable once StaticMemberInGenericType
		public static readonly World Instance;

		static World()
		{
			if (Attribute.GetCustomAttribute(typeof(TWorldType), typeof(WorldTypeAttribute)) is WorldTypeAttribute worldAttribute)
			{
				Instance = new World(new WorldConfig(
					worldAttribute.PageSize,
					worldAttribute.StoreEmptyTypesAsDataSets,
					worldAttribute.FullStability,
					worldAttribute.PackingWhenIterating));
			}
			else
			{
				Instance = new World(new WorldConfig());
			}

			Worlds.Register(typeof(TWorldType), Instance);
		}
	}
}
