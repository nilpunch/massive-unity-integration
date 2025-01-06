using System.Collections.Generic;
using Massive.Netcode;

namespace Massive.Unity
{
	public class SimulationSystemAdapter : ISimulation
	{
		private readonly SimulationTime _simulationTime;

		public List<UpdateSystem> Systems { get; } = new List<UpdateSystem>();

		public SimulationSystemAdapter(SimulationTime simulationTime)
		{
			_simulationTime = simulationTime;
		}

		public void Update(int tick)
		{
			foreach (var system in Systems)
			{
				system.UpdateFrame(_simulationTime.DeltaTime);
			}
		}
	}
}
