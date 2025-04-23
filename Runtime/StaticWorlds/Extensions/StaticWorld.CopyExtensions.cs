#if !MASSIVE_DISABLE_ASSERT
#define MASSIVE_ASSERT
#endif

using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

namespace Massive.Unity
{
	[Il2CppSetOption(Option.NullChecks, false)]
	[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
	[Il2CppSetOption(Option.DivideByZeroChecks, false)]
	public partial class StaticWorld<TWorldType>
	{
		/// <summary>
		/// Creates and returns a new world that is an exact copy of this one.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static World Clone()
		{
			var clone = new World(Instance.Config);
			Instance.CopyTo(clone);
			return clone;
		}

		/// <summary>
		/// Copies the contents of this world into the specified one.
		/// Clears sets in the target world that are not present in the source.
		/// </summary>
		/// <remarks>
		/// Throws if the worlds have incompatible configs.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CopyTo(World other)
		{
			Assert.CompatibleConfigs(Instance, other);

			// Entities.
			Instance.Entities.CopyTo(other.Entities);

			// Sets.
			Instance.SetRegistry.CopyTo(other.SetRegistry);
		}
	}
}
