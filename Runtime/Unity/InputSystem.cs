using Massive.Netcode;
using UnityEngine;

namespace Massive.Unity
{
	public abstract class InputSystem : MonoBehaviour
	{
		public abstract void Init(Simulation simulation);
		public abstract void UpdateInput(int targetTick);
	}
}
