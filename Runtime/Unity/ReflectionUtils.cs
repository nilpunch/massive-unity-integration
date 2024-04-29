using System;
using System.Linq;

namespace Massive.Unity
{
	public static class ReflectionUtils
	{
		class U<T> where T : unmanaged
		{
		}

		public static bool IsUnManaged<T>()
		{
			try
			{
				// ReSharper disable once ReturnValueOfPureMethodIsNotUsed
				typeof(U<>).MakeGenericType(typeof(T));
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public static string GetSimpleGenericTypeName(Type type)
		{
			if (type.IsGenericType)
			{
				string genericArguments = string.Join(",", type.GetGenericArguments().Select(GetSimpleGenericTypeName));
				string typeItself = type.FullName!.Substring(0, type.FullName.IndexOf("`", StringComparison.Ordinal));
				return $"{typeItself}<{genericArguments}>";
			}
			return type.FullName;
		}

		public static Type GetBaseGenericType(this Type type, Type generic)
		{
			while (type != null && type != typeof(object))
			{
				var baseType = type.BaseType;
				var genericBase = baseType.IsGenericType ? baseType.GetGenericTypeDefinition() : baseType;

				if (genericBase == generic)
				{
					return baseType;
				}

				type = baseType;
			}

			return null;
		}

		public static bool IsSubclassOfGeneric(this Type type, Type generic)
		{
			while (type != null && type != typeof(object))
			{
				var current = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
				if (generic == current)
				{
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}
	}
}
