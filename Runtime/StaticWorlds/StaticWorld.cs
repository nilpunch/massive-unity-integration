using System;
using Unity.IL2CPP.CompilerServices;

namespace Massive.Unity
{
	[Il2CppEagerStaticClassConstruction]
	public partial class StaticWorld<TWorldType>
	{
		// ReSharper disable once StaticMemberInGenericType
		public static readonly World Instance;

		static StaticWorld()
		{
			if (Attribute.GetCustomAttribute(typeof(TWorldType), typeof(StaticWorldTypeAttribute)) is StaticWorldTypeAttribute worldAttribute)
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

			StaticWorlds.Register(typeof(TWorldType), Instance);
		}
	}
}
