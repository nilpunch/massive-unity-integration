using UnityEngine;

namespace UPR
{
	[RequireComponent(typeof(MonoEntity))]
	[DisallowMultipleComponent]
	public abstract class UnmanagedComponentBase<TComponent, TMonoComponent> : MonoComponent
		where TComponent : unmanaged
		where TMonoComponent : UnmanagedComponentBase<TComponent, TMonoComponent>
	{
		public static IComponentReflector GetComponentReflector()
		{
			return new UnmanagedComponentReflector<TComponent, TMonoComponent>();
		}
	}
}
