using Massive;
using UnityEngine;

namespace UPR
{
	[RequireComponent(typeof(MonoEntity))]
	public abstract class MonoComponent : MonoBehaviour
	{
		public abstract void ApplyToEntity(IRegistry registry, Entity entity);

		public abstract void Synchronize(IRegistry registry, Entity entity);
	}
}
