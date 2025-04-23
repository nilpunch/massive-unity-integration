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
		/// Returns alive entity for this ID.
		/// </summary>
		/// <remarks>
		/// Throws if the entity with this ID is not alive.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Entity GetEntity(int id)
		{
			return Instance.Entities.GetEntity(id);
		}

		/// <summary>
		/// Creates a unique entity and returns it.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Entity CreateEntity()
		{
			return Instance.Entities.Create();
		}

		/// <summary>
		/// Creates a unique entity, adds a component without initializing data, and returns the entity.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Entity CreateEntity<T>()
		{
			var entity = Instance.CreateEntity();
			Instance.Add<T>(entity.Id);
			return entity;
		}

		/// <summary>
		/// Creates a unique entity, adds a component with provided data, and returns the entity.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Entity CreateEntity<T>(T data)
		{
			var entity = Instance.CreateEntity();
			Instance.Set(entity.Id, data);
			return entity;
		}

		/// <summary>
		/// Creates a unique entity with components of another entity.
		/// </summary>
		/// <remarks>
		/// Throws if the entity is not alive.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Entity Clone(Entity entity)
		{
			Assert.IsAlive(Instance, entity);

			var cloneId = Instance.Clone(entity.Id);
			return Instance.GetEntity(cloneId);
		}

		/// <summary>
		/// Destroys this entity.
		/// </summary>
		/// <remarks>
		/// Throws if the entity is not alive.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Destroy(Entity entity)
		{
			Assert.IsAlive(Instance, entity);

			Instance.Entities.Destroy(entity.Id);
		}

		/// <summary>
		/// Checks whether the entity is alive.
		/// </summary>
		/// <remarks>
		/// Throws if provided entity ID is negative.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsAlive(Entity entity)
		{
			return Instance.Entities.IsAlive(entity);
		}

		/// <summary>
		/// Adds a component to the entity and sets its data.
		/// </summary>
		/// <remarks>
		/// Throws if the entity is not alive.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Set<T>(Entity entity, T data)
		{
			Assert.IsAlive(Instance, entity);

			Instance.Set(entity.Id, data);
		}

		/// <summary>
		/// Adds a component to the entity without initializing data.
		/// </summary>
		/// <returns>
		/// True if the component was added; false if it was already present.
		/// </returns>
		/// <remarks>
		/// Throws if the entity is not alive.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Add<T>(Entity entity)
		{
			Assert.IsAlive(Instance, entity);

			return Instance.SparseSet<T>().Add(entity.Id);
		}

		/// <summary>
		/// Removes a component from the entity.
		/// </summary>
		/// <returns>
		/// True if the component was removed; false if it was not present.
		/// </returns>
		/// <remarks>
		/// Throws if the entity is not alive.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Remove<T>(Entity entity)
		{
			Assert.IsAlive(Instance, entity);

			return Instance.SparseSet<T>().Remove(entity.Id);
		}

		/// <summary>
		/// Checks whether the entity has such a component.
		/// </summary>
		/// <remarks>
		/// Throws if the entity is not alive.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Has<T>(Entity entity)
		{
			Assert.IsAlive(Instance, entity);

			return Instance.SparseSet<T>().Has(entity.Id);
		}

		/// <summary>
		/// Returns a reference to the component of the entity.
		/// </summary>
		/// <remarks>
		/// Throws if the entity is not alive,
		/// or if the type has no associated data set.
		/// </remarks>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ref T Get<T>(Entity entity)
		{
			Assert.IsAlive(Instance, entity);

			var dataSet = Instance.DataSet<T>();

			return ref dataSet.Get(entity.Id);
		}
	}
}
