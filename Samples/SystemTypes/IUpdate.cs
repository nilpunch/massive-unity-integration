using Massive.QoL;

namespace Massive.Unity
{
	public interface IUpdate : IRunMethod<IUpdate>
	{
		void Update();

		void IRunMethod<IUpdate>.Run() => Update();
	}
}
