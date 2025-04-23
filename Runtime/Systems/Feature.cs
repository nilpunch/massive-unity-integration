namespace Massive.Unity
{
	public class Feature
	{
		private readonly FastList<IInitialize> _initializeSystems = new FastList<IInitialize>();
		private readonly FastList<ICleanup> _cleanupSystems = new FastList<ICleanup>();
		private readonly FastList<IUpdate> _updateSystems = new FastList<IUpdate>();

		private readonly World _world;

		public Feature(World world)
		{
			_world = world;
		}

		public void AddSystem(ISystem system)
		{
			system.World = _world;

			if (system is IInitialize initializeSystem)
			{
				_initializeSystems.Add(initializeSystem);
			}
			if (system is IUpdate updateSystem)
			{
				_updateSystems.Add(updateSystem);
			}
			if (system is ICleanup cleanupSystem)
			{
				_cleanupSystems.Add(cleanupSystem);
			}
		}

		public void Initialize()
		{
			foreach (var initializeSystem in _initializeSystems)
			{
				initializeSystem.Initialize();
			}
		}

		public void Update()
		{
			foreach (var updateSystem in _updateSystems)
			{
				updateSystem.Update();
			}
		}

		public void Cleanup()
		{
			foreach (var cleanupSystem in _cleanupSystems)
			{
				cleanupSystem.Cleanup();
			}
		}
	}
}
