using System;
using Massive;

namespace UPR
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
