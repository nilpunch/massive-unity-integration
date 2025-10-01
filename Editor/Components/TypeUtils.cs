#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Massive.Unity.Editor
{
	internal static class TypeUtils
	{
		private static readonly Dictionary<Type, IReadOnlyList<Attribute>> s_attributesCache =
			new Dictionary<Type, IReadOnlyList<Attribute>>();

		private static IReadOnlyList<Assembly> s_assemblies;
		private static IReadOnlyList<Type> s_allNonAbstractTypesBackingField;

		private static readonly Dictionary<Type, string> s_typeShortNames = new Dictionary<Type, string>();
		private static readonly Dictionary<Type, string> s_typeFullNames = new Dictionary<Type, string>();
		private static readonly Dictionary<Type, string> s_typeNamespaces = new Dictionary<Type, string>();

		public static IReadOnlyList<Assembly> Assemblies
		{
			get
			{
				if (s_assemblies == null)
				{
					s_assemblies = AppDomain.CurrentDomain.GetAssemblies()
						.Where(assembly => IsUserAssembly(assembly.GetName().Name))
						.ToList();
				}
				
				return s_assemblies;
			}
		}

		public static IReadOnlyList<Type> AllNonAbstractTypes
		{
			get
			{
				if (s_allNonAbstractTypesBackingField == null)
				{
					s_allNonAbstractTypesBackingField = Assemblies
						.SelectMany(asm =>
						{
							try
							{
								return asm.GetTypes();
							}
							catch (ReflectionTypeLoadException)
							{
								return Array.Empty<Type>();
							}
						})
						.Where(type => !type.IsAbstract)
						.ToList();
				}

				return s_allNonAbstractTypesBackingField;
			}
		}

		public static IReadOnlyList<Attribute> GetAttributesCached(Type type)
		{
			if (s_attributesCache.TryGetValue(type, out var attributes))
			{
				return attributes;
			}

			return s_attributesCache[type] = type.GetCustomAttributes().ToList();
		}

		public static IReadOnlyList<T> GetCustomAttributes<T>(this Assembly asm)
		{
			return asm.GetCustomAttributes(typeof(T)).Cast<T>().ToList();
		}

		public static IReadOnlyList<FieldInfo> GetAllInstanceFieldsInDeclarationOrder(Type type)
		{
			const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
				BindingFlags.Instance | BindingFlags.DeclaredOnly;

			return GetAllMembersInDeclarationOrder(type, it => it.GetFields(flags));
		}

		public static IReadOnlyList<PropertyInfo> GetAllInstancePropertiesInDeclarationOrder(Type type)
		{
			const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
				BindingFlags.Instance | BindingFlags.DeclaredOnly;

			return GetAllMembersInDeclarationOrder(type, it => it.GetProperties(flags));
		}

		public static IReadOnlyList<MethodInfo> GetAllInstanceMethodsInDeclarationOrder(Type type)
		{
			const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
				BindingFlags.Instance | BindingFlags.DeclaredOnly;

			return GetAllMembersInDeclarationOrder(type, it => it.GetMethods(flags));
		}

		public static bool IsArrayOrList(Type type, out Type elementType)
		{
			if (type.IsArray)
			{
				elementType = type.GetElementType();
				return true;
			}

			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
			{
				elementType = type.GetGenericArguments().Single();
				return true;
			}

			elementType = null;
			return false;
		}

		public static Type GetUnityEditorTypeByFullName(string name)
		{
			return GetTypeByFullName(name, typeof(UnityEditor.Editor).Assembly);
		}

		public static Type GetTypeByFullName(string name, Assembly assembly)
		{
			return assembly
				.GetTypes()
				.Single(it => it.FullName == name);
		}

		public static bool TryFindTypeByFullName(string name, out Type type)
		{
			type = Type.GetType(name);
			if (type != null)
			{
				return true;
			}

			foreach (var assembly in Assemblies)
			{
				type = assembly.GetType(name);
				if (type != null)
				{
					return true;
				}
			}

			return false;
		}

		private static IReadOnlyList<T> GetAllMembersInDeclarationOrder<T>(
			Type type, Func<Type, T[]> select)
			where T : MemberInfo
		{
			var result = new List<T>();
			var typeTree = new Stack<Type>();

			while (type != null)
			{
				typeTree.Push(type);
				type = type.BaseType;
			}

			foreach (var t in typeTree)
			{
				var items = select(t);
				result.AddRange(items);
			}

			return result;
		}

		public static string GetShortName(Type type)
		{
			EnsureRegistered(type);
			return s_typeShortNames[type];
		}

		public static string GetFullName(Type type)
		{
			EnsureRegistered(type);
			return s_typeFullNames[type];
		}

		public static string GetNamespace(Type type)
		{
			EnsureRegistered(type);
			return s_typeNamespaces[type];
		}

		private static void EnsureRegistered(Type type)
		{
			if (s_typeFullNames.ContainsKey(type))
			{
				return;
			}

			var fullName = type.GetFullGenericName();

			var lastDot = fullName.LastIndexOf('.');
			var shortName = lastDot >= 0 ? fullName.Substring(lastDot + 1) : fullName;
			var nameSpace = lastDot >= 0 ? fullName.Substring(0, lastDot) : "";
			s_typeFullNames.Add(type, fullName);
			s_typeShortNames.Add(type, shortName);
			s_typeNamespaces.Add(type, nameSpace);
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
				!assemblyName.StartsWith("ReportGeneratorMerged") &&
				!assemblyName.StartsWith("mscorlib") &&
				!assemblyName.StartsWith("Mono.") &&
				!assemblyName.StartsWith("JetBrains.") &&
				!assemblyName.StartsWith("log4net") &&
				!assemblyName.StartsWith("nunit.framework") &&
				!assemblyName.StartsWith("unityplastic");
		}
	}
}
#endif