using System;

namespace Massive.Unity
{
	[AttributeUsage(AttributeTargets.Struct)]
	public class StaticWorldTypeAttribute : Attribute
	{
		public readonly int PageSize;

		public readonly bool StoreEmptyTypesAsDataSets;

		public readonly bool FullStability;

		public readonly Packing PackingWhenIterating;

		public StaticWorldTypeAttribute(int pageSize = Constants.DefaultPageSize, bool storeEmptyTypesAsDataSets = false,
			bool fullStability = false, Packing packingWhenIterating = Packing.WithHoles)
		{
			PageSize = pageSize;
			StoreEmptyTypesAsDataSets = storeEmptyTypesAsDataSets;
			FullStability = fullStability;
			PackingWhenIterating = packingWhenIterating;
		}
	}
}
