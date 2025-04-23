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
		public static View View()
		{
			return new View(Instance, Instance.Config.PackingWhenIterating);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static View View(Packing packingWhenIterating)
		{
			return new View(Instance, packingWhenIterating);
		}
	}
}
