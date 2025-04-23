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
		/// Creates a unique entity and returns its ID.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Create()
		{
			return Instance.Entities.Create().Id;
		}

		/// <summary>
		/// Creates a unique entity, adds a component without initializing data, and returns the entity ID.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Create<T>()
		{
			var id = Instance.Create();
			Instance.Add<T>(id);
			return id;
		}

		/// <summary>
		/// Creates a unique entity, adds a component with provided data, and returns the entity ID.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Create<T>(T data)
		{
			var id = Instance.Create();
			Instance.Set(id, data);
			return id;
		}

		/// <summary>
		/// Creates a unique entity with components of another entity and returns the entity ID.
		/// </summary>
		/// <remarks>
		/// Throws if the entity with this ID is not alive.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Clone(int id)
		{
			Assert.IsAlive(Instance, id);

			var cloneId = Instance.Create();

			var setList = Instance.SetRegistry.AllSets;
			var setCount = setList.Count;
			var sets = setList.Items;
			for (var i = 0; i < setCount; i++)
			{
				var set = sets[i];
				var index = set.GetIndexOrNegative(id);
				if (index >= 0)
				{
					set.Add(cloneId);
					var cloneIndex = set.Sparse[cloneId];
					set.CopyDataAt(index, cloneIndex);
				}
			}

			return cloneId;
		}

		/// <summary>
		/// Destroys any alive entity with this ID.
		/// </summary>
		/// <remarks>
		/// Throws if the entity with this ID is not alive.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Destroy(int id)
		{
			Assert.IsAlive(Instance, id);

			Instance.Entities.Destroy(id);
		}

		/// <summary>
		/// Checks whether the entity with this ID is alive.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAlive(int id)
		{
			return Instance.Entities.IsAlive(id);
		}

		/// <summary>
		/// Adds a component to the entity with this ID and sets its data.
		/// </summary>
		/// <remarks>
		/// Throws if the entity with this ID is not alive.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Set<T>(int id, T data)
		{
			Assert.IsAlive(Instance, id);

			var set = Instance.SparseSet<T>();
			set.Add(id);
			if (set is DataSet<T> dataSet)
			{
				dataSet.Get(id) = data;
			}
		}

		/// <summary>
		/// Adds a component to the entity with this ID without initializing data.
		/// </summary>
		/// <returns>
		/// True if the component was added; false if it was already present.
		/// </returns>
		/// <remarks>
		/// Throws if the entity with this ID is not alive.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Add<T>(int id)
		{
			Assert.IsAlive(Instance, id);

			return Instance.SparseSet<T>().Add(id);
		}

		/// <summary>
		/// Removes a component from the entity with this ID.
		/// </summary>
		/// <returns>
		/// True if the component was removed; false if it was not present.
		/// </returns>
		/// <remarks>
		/// Throws if the entity with this ID is not alive.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Remove<T>(int id)
		{
			Assert.IsAlive(Instance, id);

			return Instance.SparseSet<T>().Remove(id);
		}

		/// <summary>
		/// Checks whether the entity with this ID has such a component.
		/// </summary>
		/// <remarks>
		/// Throws if the entity with this ID is not alive.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has<T>(int id)
		{
			Assert.IsAlive(Instance, id);

			return Instance.SparseSet<T>().Has(id);
		}

		/// <summary>
		/// Returns a reference to the component of the entity with this ID.
		/// </summary>
		/// <remarks>
		/// Throws if the entity with this ID is not alive,
		/// or if the type has no associated data set.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T Get<T>(int id)
		{
			Assert.IsAlive(Instance, id);

			var dataSet = Instance.DataSet<T>();

			return ref dataSet.Get(id);
		}
	}
}
