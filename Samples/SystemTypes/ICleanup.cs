using Massive.QoL;

namespace Massive.Unity
{
	public interface ICleanup : IRunMethod<ICleanup>
	{
		void Cleanup();

		void IRunMethod<ICleanup>.Run() => Cleanup();
	}
}
