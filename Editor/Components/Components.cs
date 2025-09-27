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
		private static readonly Dictionary<Type, string> s_nameOfComponents = new Dictionary<Type, string>();
		private static readonly Dictionary<Type, string> s_fullNameOfComponents = new Dictionary<Type, string>();
		private static readonly Dictionary<Type, string> s_namespaceOfComponents = new Dictionary<Type, string>();
		private static readonly FastList<string> s_fullComponentNames = new FastList<string>();
		private static readonly FastList<Type> s_components = new FastList<Type>();
		private static bool _warmedUp;

		public static ReadOnlySpan<string> AllComponentNames
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => s_fullComponentNames.ReadOnlySpan;
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
				var name = assembly.GetName().Name;
				if (!IsUserAssembly(name))
				{
					continue;
				}

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
					if ((type.IsValueType || type.GetConstructor(Type.EmptyTypes) != null)
						&& type.IsDefined(typeof(SerializableAttribute), false)
						&& type.IsDefined(typeof(ComponentAttribute), false)
						&& !type.Name.Contains("<")
						&& !type.IsGenericTypeDefinition)
					{
						Register(type);
					}
				}
			}
			_warmedUp = true;
		}

		public static string GetShortName(Type type)
		{
			return s_nameOfComponents[type];
		}

		public static string GetFullName(Type type)
		{
			return s_fullNameOfComponents[type];
		}

		public static string GetNamespace(Type type)
		{
			return s_namespaceOfComponents[type];
		}

		internal static void Register(Type componentType)
		{
			var fullName = componentType.GetFullGenericName();
			var componentIndex = s_fullComponentNames.BinarySearch(fullName);
			if (componentIndex > 0)
			{
				throw new Exception($"Duplicate of {fullName}.");
			}
			var insertionIndex = ~componentIndex;
			s_fullComponentNames.Insert(insertionIndex, fullName);
			s_components.Insert(insertionIndex, componentType);
			s_componentByName.Add(fullName, componentType);

			var lastDot = fullName.LastIndexOf('.');
			var shortName = lastDot >= 0 ? fullName.Substring(lastDot + 1) : fullName;
			var nameSpace = lastDot >= 0 ? fullName.Substring(0, lastDot) : "";
			s_fullNameOfComponents.Add(componentType, fullName);
			s_nameOfComponents.Add(componentType, shortName);
			s_namespaceOfComponents.Add(componentType, nameSpace);
		}

		private static bool IsUserAssembly(string assemblyName)
		{
			// Default Unity assemblies compiled from Assets/
			if (assemblyName == "Assembly-CSharp" || assemblyName == "Assembly-CSharp-Editor")
			{
				return true;
			}

			// Include .asmdef-based assemblies
			return !assemblyName.StartsWith("Assembly") &&
				!assemblyName.StartsWith("Unity.") &&
				!assemblyName.StartsWith("UnityEngine") &&
				!assemblyName.StartsWith("UnityEditor") &&
				!assemblyName.StartsWith("System") &&
				!assemblyName.StartsWith("mscorlib") &&
				!assemblyName.StartsWith("Mono.") &&
				!assemblyName.StartsWith("JetBrains.") &&
				!assemblyName.StartsWith("log4net") &&
				!assemblyName.StartsWith("unityplastic");
		}
	}
}
#endif
