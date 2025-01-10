using Massive.Netcode;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public class MoveSystem : UpdateSystem
	{
		[SerializeField] private float _speed = 5f;
		[SerializeField] private FadeOutConfig _inputFadeOut;

		private SimulationInput _input;
		private Registry _registry;

		public override void Init(Registry registry)
		{
			_registry = registry;
			_input = _registry.Service<SimulationInput>();
		}

		public override void UpdateFrame(float deltaTime)
		{
			var transforms = _registry.DataSet<LocalTransform>();
			var players = _registry.DataSet<Player>();
			foreach (var entity in _registry.View().Include<Player, LocalTransform>())
			{
				var clientId = players.Get(entity).ClientId;
				var input = _input.Get<MoveInput>(clientId).FadeOut(_inputFadeOut);

				transforms.Get(entity).Position += input.Direction * deltaTime * _speed;
			}
		}
	}
}
