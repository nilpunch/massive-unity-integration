using UnityEngine;

namespace Massive.Unity
{
	public interface IComponentConverter
	{
		void ApplyComponent(Entity entity, Transform transform);
	}
}
