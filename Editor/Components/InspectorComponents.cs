#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace Massive.Unity.Editor
{
	/// <summary>
	/// Used to explicitly add components to the component selector in editor.
	/// </summary>
	public static class InspectorComponents
	{
		private static readonly HashSet<Type> _types = new HashSet<Type>();

		public static bool Has(Type type)
		{
			return type != null && _types.Contains(type);
		}

		public static void Add(Type type)
		{
			if (type != null)
			{
				_types.Add(type);
			}
		}
	}
}
#endif