using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

#if UNITY_EDITOR
namespace Massive.Unity.Editor
{
	public static class Components
	{
		private static readonly Dictionary<string, Type> s_componentByName = new Dictionary<string, Type>();
		private static readonly FastList<string> s_componentNames = new FastList<string>();
		private static readonly FastList<Type> s_components = new FastList<Type>();
		private static bool _warmedUp;

		public static ReadOnlySpan<string> AllComponentNames
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => s_componentNames.ReadOnlySpan;
		}

		public static ReadOnlySpan<Type> AllComponentTypes
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => s_components.ReadOnlySpan;
		}

		public static void Warmup()
		{
			if (_warmedUp)
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
					if (type.IsDefined(typeof(ComponentAttribute), false))
					{
						Register(type);
					}
				}
			}
			_warmedUp = true;
		}

		internal static void Register(Type componentType)
		{
			var worldTypeName = componentType.GetFullGenericName();
			var worldIndex = s_componentNames.BinarySearch(componentType.GetFullGenericName());
			var insertionIndex = ~worldIndex;
			s_componentNames.Insert(insertionIndex, worldTypeName);
			s_components.Insert(insertionIndex, componentType);
			s_componentByName.Add(worldTypeName, componentType);
		}
	}
}
#endif