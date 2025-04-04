using Mathematics.Fixed;
using UnityEngine;

namespace Massive.Unity
{
	public abstract class UpdateSystem : MonoBehaviour
	{
		public abstract void Init(ServiceLocator serviceLocator);
		public abstract void UpdateFrame(FP deltaTime);
	}
}
