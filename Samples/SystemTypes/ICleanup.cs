using Massive.QoL;

namespace Massive.Unity
{
	public interface ICleanup : ISystemMethod<ICleanup>
	{
		void Cleanup();

		void ISystemMethod<ICleanup>.Run() => Cleanup();
	}
}
