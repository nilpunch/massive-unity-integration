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
		private static readonly HashSet<Type> _typesToShow = new HashSet<Type>();
		private static readonly HashSet<Type> _typesToHide = new HashSet<Type>();

		public static bool MustShow(Type type)
		{
			return type != null && _typesToShow.Contains(type);
		}

		public static bool MustHide(Type type)
		{
			return type != null && _typesToHide.Contains(type);
		}

		public static void Show(Type type)
		{
			if (type != null)
			{
				_typesToHide.Remove(type);
				_typesToShow.Add(type);
			}
		}

		public static void Hide(Type type)
		{
			if (type != null)
			{
				_typesToHide.Add(type);
				_typesToShow.Remove(type);
			}
		}
	}
}
#endif