namespace Massive.Unity
{
	public abstract class RollbackUpdateSystem : UpdateSystem
	{
		public abstract void SaveFrame();
		public abstract void Rollback(int frames);
	}
}
