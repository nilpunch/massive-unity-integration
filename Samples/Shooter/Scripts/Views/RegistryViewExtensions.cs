using System.Collections.Generic;

namespace Massive
{
	public static class RegistryViewExtensions
	{
		public static View View(this IRegistry registry)
		{
			return new View(registry);
		}

		public static View<T> View<T>(this IRegistry registry)
		{
			return new View<T>(registry);
		}

		public static View<T1, T2> View<T1, T2>(this IRegistry registry)
		{
			return new View<T1, T2>(registry);
		}

		public static View<T1, T2, T3> View<T1, T2, T3>(this IRegistry registry)
		{
			return new View<T1, T2, T3>(registry);
		}

		public static FilterView FilterView(this IRegistry registry, IFilter filter = null)
		{
			return new FilterView(registry, filter);
		}

		public static FilterView<T> FilterView<T>(this IRegistry registry, IFilter filter = null)
		{
			return new FilterView<T>(registry, filter);
		}

		public static FilterView<T1, T2> FilterView<T1, T2>(this IRegistry registry, IFilter filter = null)
		{
			return new FilterView<T1, T2>(registry, filter);
		}

		public static FilterView<T1, T2, T3> FilterView<T1, T2, T3>(this IRegistry registry, IFilter filter = null)
		{
			return new FilterView<T1, T2, T3>(registry, filter);
		}

		public static GroupView GroupView(this IRegistry registry, IGroup group)
		{
			return new GroupView(group);
		}

		public static GroupView<T> GroupView<T>(this IRegistry registry, IGroup group)
		{
			return new GroupView<T>(registry, group);
		}

		public static GroupView<T1, T2> GroupView<T1, T2>(this IRegistry registry, IGroup group)
		{
			return new GroupView<T1, T2>(registry, group);
		}

		public static GroupView<T1, T2, T3> GroupView<T1, T2, T3>(this IRegistry registry, IGroup group)
		{
			return new GroupView<T1, T2, T3>(registry, group);
		}
	}
}
