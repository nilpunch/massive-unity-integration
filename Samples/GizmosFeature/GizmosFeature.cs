using Massive.QoL;

namespace Massive.Unity.Samples.Farm
{
	public interface IDrawGizmos : ISystemMethod<IDrawGizmos>
	{
		void OnDrawGizmos();

		void ISystemMethod<IDrawGizmos>.Run() => OnDrawGizmos();
	}
}
