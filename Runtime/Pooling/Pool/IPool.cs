namespace Massive.Unity
{
	public interface IPool<TItem> : IPoolReturn<TItem>
	{
		TItem Get();
	}
}
