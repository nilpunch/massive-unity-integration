using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Massive.Unity
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public partial class StaticWorld<TWorldType>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Filter Filter<TInclude>()
			where TInclude : IIncludeSelector, new()
		{
			return Instance.FilterRegistry.Get<TInclude, None>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Filter Filter<TInclude, TExclude>()
			where TInclude : IIncludeSelector, new()
			where TExclude : IExcludeSelector, new()
		{
			return Instance.FilterRegistry.Get<TInclude, TExclude>();
		}
	}
}
