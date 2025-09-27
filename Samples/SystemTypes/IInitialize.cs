using Massive.QoL;

namespace Massive.Unity
{
	public interface IInitialize : ISystemMethod<IInitialize>
	{
		void Initialize();

		void ISystemMethod<IInitialize>.Run() => Initialize();
	}
}
