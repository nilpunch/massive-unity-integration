using System;

namespace Massive.Unity
{
	[Serializable]
	internal struct ManagedSample : IManaged<ManagedSample>
	{
		public string String;

		public void CopyTo(ref ManagedSample other)
		{
			other = this;
		}
	}
}
