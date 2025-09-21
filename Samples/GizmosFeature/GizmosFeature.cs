using Massive.QoL;

namespace Massive.Unity.Samples.Farm
{
	public interface IDrawGizmos : IRunMethod<IDrawGizmos>
	{
		void OnDrawGizmos();

		void IRunMethod<IDrawGizmos>.Run() => OnDrawGizmos();
	}
}
