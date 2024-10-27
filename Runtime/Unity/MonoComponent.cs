using UnityEngine;

namespace Massive.Unity
{
	[RequireComponent(typeof(MonoEntity))]
	public abstract class MonoComponent : MonoBehaviour
	{
		public abstract void ApplyToEntity(Registry registry, Entity entity);
	}

	[RequireComponent(typeof(MonoEntity))]
	[DisallowMultipleComponent]
	public class MonoComponent<TComponent> : MonoComponent
	{
		[SerializeField] private TComponent _data;

		public override void ApplyToEntity(Registry registry, Entity entity)
		{
			registry.Assign(entity, _data);
		}
	}
}
