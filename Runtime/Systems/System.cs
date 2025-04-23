using System.Runtime.CompilerServices;

namespace Massive.Unity
{
	public class System : ISystem
	{
		public World World { get; set; }

		public View View
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => World.View();
		}
	}
}
