using Massive.Netcode;
using UnityEngine;

namespace Massive.Unity
{
	public abstract class UpdateSystem : MonoBehaviour
	{
		public abstract void Init(Simulation simulation);
		public abstract void UpdateFrame(float deltaTime);
	}
}
