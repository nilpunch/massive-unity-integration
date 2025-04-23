using System.Runtime.CompilerServices;
using UnityEngine;

namespace Massive.Unity
{
	public abstract class ScriptableSystem : ScriptableObject, ISystem
	{
		public World World { get; set; }

		public View View
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => World.View();
		}
	}
}
