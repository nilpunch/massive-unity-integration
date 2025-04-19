using System;

namespace Massive.Unity
{
	[AttributeUsage(AttributeTargets.Struct)]
	public class WorldTypeAttribute : Attribute
	{
		public readonly int PageSize;

		public readonly bool StoreEmptyTypesAsDataSets;

		public readonly bool FullStability;

		public readonly Packing PackingWhenIterating;

		public WorldTypeAttribute(int pageSize = Constants.DefaultPageSize, bool storeEmptyTypesAsDataSets = false,
			bool fullStability = false, Packing packingWhenIterating = Packing.WithHoles)
		{
			PageSize = pageSize;
			StoreEmptyTypesAsDataSets = storeEmptyTypesAsDataSets;
			FullStability = fullStability;
			PackingWhenIterating = packingWhenIterating;
		}
	}
}
