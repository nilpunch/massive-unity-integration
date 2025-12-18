namespace Massive.Unity
{
	public interface IInitialize : ISystemMethod<IInitialize>
	{
		void Initialize();

		void ISystemMethod<IInitialize>.Run() => Initialize();
	}
}
