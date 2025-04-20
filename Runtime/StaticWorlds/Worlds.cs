using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Massive.Unity
{
	[Il2CppEagerStaticClassConstruction]
	public static class Worlds
	{
		private static readonly Dictionary<Type, World> s_worldByType = new Dictionary<Type, World>();
		private static readonly Dictionary<string, World> s_worldByTypeName = new Dictionary<string, World>();
		private static readonly FastList<string> s_worldTypeNames = new FastList<string>();
		private static readonly FastList<World> s_worlds = new FastList<World>();
		private static bool _warmedUpAll;

		public static ReadOnlySpan<string> AllWorldsNames
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => s_worldTypeNames.ReadOnlySpan;
		}

		public static ReadOnlySpan<World> AllWorlds
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => s_worlds.ReadOnlySpan;
		}

		public static World GetWorld(Type worldType)
		{
			if (!s_worldByType.TryGetValue(worldType, out var world))
			{
				WarmupWorld(worldType);
				world = s_worldByType[worldType];
			}

			return world;
		}

		public static string GetWorldName(World world)
		{
			return s_worldTypeNames[s_worlds.IndexOf(world)];
		}

		public static bool TryGetWorldByName(string worldTypeName, out World world)
		{
			return s_worldByTypeName.TryGetValue(worldTypeName, out world);
		}

		public static bool TryGetWorldName(World world, out string worldTypeName)
		{
			var index = s_worlds.IndexOf(world);
			if (index < 0)
			{
				worldTypeName = null;
				return false;
			}

			worldTypeName = s_worldTypeNames[index];
			return true;
		}

		public static void WarmupAll()
		{
			if (_warmedUpAll)
			{
				return;
			}

			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				Type[] types;
				try
				{
					types = assembly.GetTypes();
				}
				catch (ReflectionTypeLoadException exception)
				{
					types = exception.Types.Where(t => t != null).ToArray();
				}

				foreach (var type in types)
				{
					if (type.IsDefined(typeof(WorldTypeAttribute), false))
					{
						WarmupWorld(type);
					}
				}
			}
			_warmedUpAll = true;
		}

		public static void WarmupWorld(Type worldType)
		{
			try
			{
				var concreteWorld = typeof(World<>).MakeGenericType(worldType);
				RuntimeHelpers.RunClassConstructor(concreteWorld.TypeHandle);
			}
			catch
			{
				throw new Exception(
					$"The static world for {worldType.GetFullGenericName()} has been stripped from the build.\n" +
					"Ensure that this static world is used in your codebase.");
			}
		}

		internal static void Register(Type worldType, World world)
		{
			var worldTypeName = worldType.GetFullGenericName();
			var worldIndex = s_worldTypeNames.BinarySearch(worldTypeName);
			var insertionIndex = ~worldIndex;
			s_worldTypeNames.Insert(insertionIndex, worldTypeName);
			s_worlds.Insert(insertionIndex, world);

			s_worldByType.Add(worldType, world);
			s_worldByTypeName.Add(worldTypeName, world);
		}
	}
}
