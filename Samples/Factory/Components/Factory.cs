using System;
using Fixed32;

namespace Massive.Unity.Samples
{
	[Serializable, Component]
	public struct Factory
	{
		public FP ProductionProgress;

		public ProductionUnit ProductionUnit;
	}
}
