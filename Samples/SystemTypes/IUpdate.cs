namespace Massive.Unity
{
	public interface IUpdate : ISystemMethod<IUpdate>
	{
		void Update();

		void ISystemMethod<IUpdate>.Run() => Update();
	}
}
