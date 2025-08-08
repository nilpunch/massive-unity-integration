using System.Runtime.CompilerServices;

namespace Massive.Unity
{
	public abstract class FilterSystem : System, IUpdate, IEntityAction
	{
		protected virtual Filter Filter { get; } = new Filter();

		public void Update()
		{
			var system = this;
			var filter = Filter;
			if (filter.IncludedCount == 0 && filter.ExcludedCount == 0)
			{
				View.ForEach(ref system);
			}
			else
			{
				View.Filter(filter).ForEach(ref system);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool IEntityAction.Apply(int id)
		{
			Process(id);
			return true;
		}

		protected abstract void Process(int id);
	}

	public abstract class FilterSystem<T> : System, IUpdate, IEntityAction<T>
	{
		protected virtual Filter Filter { get; } = new Filter();

		public void Update()
		{
			var system = this;
			var filter = Filter;
			if (filter.IncludedCount == 0 && filter.ExcludedCount == 0)
			{
				View.ForEach<FilterSystem<T>, T>(ref system);
			}
			else
			{
				View.Filter(filter).ForEach<FilterSystem<T>, T>(ref system);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool IEntityAction<T>.Apply(int id, ref T a)
		{
			Process(id, ref a);
			return true;
		}

		protected abstract void Process(int entity, ref T a);
	}

	public abstract class FilterSystem<T1, T2> : System, IUpdate, IEntityAction<T1, T2>
	{
		protected virtual Filter Filter { get; } = new Filter();

		public void Update()
		{
			var system = this;
			var filter = Filter;
			if (filter.IncludedCount == 0 && filter.ExcludedCount == 0)
			{
				View.ForEach<FilterSystem<T1, T2>, T1, T2>(ref system);
			}
			else
			{
				View.Filter(filter).ForEach<FilterSystem<T1, T2>, T1, T2>(ref system);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool IEntityAction<T1, T2>.Apply(int id, ref T1 a, ref T2 b)
		{
			Process(id, ref a, ref b);
			return true;
		}

		protected abstract void Process(int entity, ref T1 a, ref T2 b);
	}
}
