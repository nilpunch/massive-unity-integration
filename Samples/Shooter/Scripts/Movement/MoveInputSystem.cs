using Massive.Netcode;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public class MoveInputSystem : InputSystem
	{
		[SerializeField] private int _clientId = 1;
		[SerializeField] private int _tickOffset = 0;
		
		private Simulation _simulation;

		public override void Init(Simulation simulation)
		{
			_simulation = simulation;
		}

		public override void UpdateInput(int targetTick)
		{
			// Simulate prediction.
			if (Input.GetKey(KeyCode.Space))
			{
				return;
			}
			
			Vector3 input = Vector3.zero;

			if (Input.GetKey(KeyCode.A))
			{
				input += Vector3.left;
			}
			if (Input.GetKey(KeyCode.D))
			{
				input += Vector3.right;
			}
			if (Input.GetKey(KeyCode.W))
			{
				input += Vector3.up;
			}
			if (Input.GetKey(KeyCode.S))
			{
				input += Vector3.down;
			}

			input.Normalize();

			_simulation.Input.SetAt(targetTick + _tickOffset, _clientId, new MoveInput() { Direction = input });
		}
	}
}
