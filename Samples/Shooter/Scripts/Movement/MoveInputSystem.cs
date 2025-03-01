using Massive.Netcode;
using UnityEngine;
using Input = UnityEngine.Input;

namespace Massive.Unity.Samples.Shooter
{
	public class MoveInputSystem : InputSystem
	{
		[SerializeField] private int _clientId = 1;

		private InputRegistry _input;

		public override void Init(InputRegistry input)
		{
			_input = input;
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

			_input.SetAt(Mathf.Max(0, targetTick - 120), _clientId, new MoveInput() { Direction = input });
			// _input.SetAt(targetTick, _clientId, new MoveInput() { Direction = input });
		}
	}
}
