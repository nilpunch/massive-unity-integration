namespace Massive.Unity
{
	public class Feature
	{
		private readonly FastList<IInitialize> _initializeSystems = new FastList<IInitialize>();
		private readonly FastList<IUpdate> _updateSystems = new FastList<IUpdate>();
		private readonly FastList<ICleanup> _cleanupSystems = new FastList<ICleanup>();

		public World World { get; }

		public Feature(World world)
		{
			World = world;
		}

		public void AddSystem(ISystem system)
		{
			system.World = World;

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

			OnSystemAdded(system);
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

		protected virtual void OnSystemAdded(ISystem system)
		{
		}
	}
}
