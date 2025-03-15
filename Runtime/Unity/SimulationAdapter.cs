using System.Collections.Generic;
using Massive.Netcode;
using Mathematics.Fixed;

namespace Massive.Unity
{
	public class SimulationAdapter : ISimulation
	{
		private readonly Time _time;

		public List<UpdateSystem> Systems { get; } = new List<UpdateSystem>();

		public SimulationAdapter(Time time)
		{
			_time = time;
		}

		public void Update(int tick)
		{
			foreach (var system in Systems)
			{
				system.UpdateFrame(FP.One / _time.FPS);
			}
		}
	}
}
