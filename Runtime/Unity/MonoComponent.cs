using UnityEngine;

namespace Massive.Unity
{
	[RequireComponent(typeof(MonoEntity))]
	public abstract class MonoComponent : MonoBehaviour
	{
		public abstract void ApplyToEntity(Registry registry, Entity entity);

		public abstract void Synchronize(Registry registry, Entity entity);

		public abstract void UnassignComponent();
	}
}
