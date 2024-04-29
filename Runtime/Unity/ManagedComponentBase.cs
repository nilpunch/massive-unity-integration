using Massive;
using UnityEngine;

namespace UPR
{
	[RequireComponent(typeof(MonoEntity))]
	[DisallowMultipleComponent]
	public abstract class ManagedComponentBase<TComponent, TMonoComponent> : MonoComponent
		where TComponent : IManaged<TComponent>
		where TMonoComponent : ManagedComponentBase<TComponent, TMonoComponent>
	{
		public static IComponentReflector GetComponentReflector()
		{
			return new ManagedComponentReflector<TComponent, TMonoComponent>();
		}
	}
}
