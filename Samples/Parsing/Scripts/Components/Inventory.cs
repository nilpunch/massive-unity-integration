using System;
using System.Collections.Generic;
using Massive;

namespace UPR
{
	[Serializable]
	public struct Inventory : IManaged<Inventory>
	{
		public List<int> Items;

		public void CopyTo(ref Inventory other)
		{
			other.Items ??= new List<int>();
			other.Items.Clear();
			other.Items.AddRange(Items);
		}
	}
}