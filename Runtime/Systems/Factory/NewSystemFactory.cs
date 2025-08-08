namespace Massive.Unity
{
	public class NewSystemFactory<T> : ISystemFactory where T : ISystem, new()
	{
		public ISystem Create()
		{
			return new T();
		}
	}
}
