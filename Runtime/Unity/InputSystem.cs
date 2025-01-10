using Massive.Netcode;
using UnityEngine;

namespace Massive.Unity
{
	public abstract class InputSystem : MonoBehaviour
	{
		public abstract void Init(InputRegistry input);
		public abstract void UpdateInput(int targetTick);
	}
}
