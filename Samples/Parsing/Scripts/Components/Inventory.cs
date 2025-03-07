﻿using System;
using System.Collections.Generic;

namespace Massive.Unity
{
	[Serializable]
	public struct Inventory : ICopyable<Inventory>
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