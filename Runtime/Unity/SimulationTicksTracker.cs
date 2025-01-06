using Massive.Netcode;

namespace Massive.Unity
{
	public class SimulationTicksTracker : ISimulation
	{
		public int TicksAmount { get; private set; }

		public void Update(int tick)
		{
			TicksAmount += 1;
		}

		public void Restart()
		{
			TicksAmount = 0;
		}
	}
}
