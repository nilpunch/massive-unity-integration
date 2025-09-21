using Massive.QoL;

namespace Massive.Unity
{
	public interface IInitialize : IRunMethod<IInitialize>
	{
		void Initialize();

		void IRunMethod<IInitialize>.Run() => Initialize();
	}
}
