namespace Massive.Unity
{
	public class FeatureFactory
	{
		private readonly FastList<ISystemFactory> _systemFactories = new FastList<ISystemFactory>();

		public FeatureFactory AddFactory(ISystemFactory systemFactory)
		{
			_systemFactories.Add(systemFactory);
			return this;
		}

		public FeatureFactory AddNew<T>() where T : ISystem, new()
		{
			_systemFactories.Add(new NewSystemFactory<T>());
			return this;
		}

		public FeatureFactory AddInstance(ISystem system)
		{
			_systemFactories.Add(new InstanceSystemFactory(system));
			return this;
		}

		public Feature CreateFeature(World world)
		{
			var feature = new Feature(world);
			foreach (var systemFactory in _systemFactories)
			{
				feature.AddSystem(systemFactory.Create());
			}
			return feature;
		}
	}
}
