using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Massive.Unity
{
	public static class ComponentReflectors
	{
		public static IReadOnlyList<IComponentReflector> All { get; }

		static ComponentReflectors()
		{
			All = GetAllReflectors();
		}

		private static IComponentReflector[] GetAllReflectors()
		{
			var monoReflections = new List<IComponentReflector>();

			var unmanagedComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(t => t.IsSubclassOfGeneric(typeof(UnmanagedComponentBase<,>)) && !t.IsGenericType)
				.ToArray();

			foreach (var monoComponentType in unmanagedComponentTypes)
			{
				var baseType = monoComponentType.GetBaseGenericType(typeof(UnmanagedComponentBase<,>));

				var methodInfo = baseType.GetMethod(
					nameof(UnmanagedComponentBase<UnmanagedSample, UnmanagedSampleComponent>.GetComponentReflector),
					BindingFlags.Public | BindingFlags.Static);

				var bindingDelegate = Delegate.CreateDelegate(typeof(Func<IComponentReflector>), methodInfo!);
				monoReflections.Add(((Func<IComponentReflector>)bindingDelegate).Invoke());
			}

			var managedComponentTypes = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(assembly => assembly.GetTypes())
				.Where(t => t.IsSubclassOfGeneric(typeof(ManagedComponentBase<,>)) && !t.IsGenericType)
				.ToArray();

			foreach (var componentType in managedComponentTypes)
			{
				var baseType = componentType.GetBaseGenericType(typeof(ManagedComponentBase<,>));

				var methodInfo = baseType.GetMethod(
					nameof(ManagedComponentBase<ManagedSample, ManagedSampleComponent>.GetComponentReflector),
					BindingFlags.Public | BindingFlags.Static);

				var bindingDelegate = Delegate.CreateDelegate(typeof(Func<IComponentReflector>), methodInfo!);
				monoReflections.Add(((Func<IComponentReflector>)bindingDelegate).Invoke());
			}

			return monoReflections.ToArray();
		}
	}
}
