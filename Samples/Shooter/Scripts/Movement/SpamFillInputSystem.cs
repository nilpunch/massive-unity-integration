using Massive.Netcode;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public class SpamFillInputSystem : InputSystem
	{
		[SerializeField] private int _clients = 100;

		private InputRegistry _input;

		public override void Init(InputRegistry input)
		{
			_input = input;

			for (int i = 0; i < _clients; i++)
			{
				_input.SetAt(0, i, new MoveInput());
			}
		}

		public override void UpdateInput(int targetTick)
		{
			
		}
	}
}
