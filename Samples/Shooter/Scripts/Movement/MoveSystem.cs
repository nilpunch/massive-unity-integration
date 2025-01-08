using Massive.Netcode;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public class MoveSystem : UpdateSystem
	{
		[SerializeField] private float _speed = 5f;
		[SerializeField] private FadeOutConfig _inputFadeOut;
		
		private Simulation _simulation;

		public override void Init(Simulation simulation)
		{
			_simulation = simulation;
		}

		public override void UpdateFrame(float deltaTime)
		{
			var transforms = _simulation.Registry.DataSet<LocalTransform>();
			var players = _simulation.Registry.DataSet<Player>();
			foreach (var entity in _simulation.Registry.View().Include<Player, LocalTransform>())
			{
				var clientId = players.Get(entity).ClientId;
				var input = _simulation.Input.Get<MoveInput>(clientId).FadeOut(_inputFadeOut);

				transforms.Get(entity).Position += input.Direction * deltaTime * _speed;
			}
		}
	}
}
