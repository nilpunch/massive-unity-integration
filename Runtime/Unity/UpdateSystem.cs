using UnityEngine;

namespace Massive.Unity
{
	public abstract class UpdateSystem : MonoBehaviour
	{
		public abstract void Init(Registry registry);
		public abstract void UpdateFrame(float deltaTime);
	}
}
