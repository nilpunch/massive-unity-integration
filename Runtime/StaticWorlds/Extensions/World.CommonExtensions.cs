using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Massive.Unity
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public partial class World<TWorldType>
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Clear()
		{
			Instance.Entities.Clear();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Clear<T>()
		{
			Instance.SparseSet<T>().Clear();
		}
	}
}
