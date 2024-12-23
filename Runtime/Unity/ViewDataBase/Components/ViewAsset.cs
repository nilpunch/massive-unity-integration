using System;
using System.Runtime.CompilerServices;

namespace Massive.Unity
{
	[Serializable]
	public struct ViewAsset : IEquatable<ViewAsset>
	{
		public int Id;

		public ViewAsset(int id)
		{
			Id = id;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(ViewAsset other)
		{
			return Id == other.Id;
		}

		public override bool Equals(object obj)
		{
			return obj is ViewAsset other && Equals(other);
		}

		public override int GetHashCode()
		{
			return Id;
		}
	}
}
