using Massive.Netcode;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public class MoveSystem : UpdateSystem
	{
		[SerializeField] private float _speed = 5f;
		[SerializeField] private FadeOutConfig _inputFadeOut;

		private Inputs _inputs;
		private World _world;

		public override void Init(ServiceLocator serviceLocator)
		{
			_world = serviceLocator.Find<World>();
			_inputs = serviceLocator.Find<Inputs>();
		}

		public override void UpdateFrame(float deltaTime)
		{
			var transforms = _world.DataSet<LocalTransform>();
			var players = _world.DataSet<Player>();
			foreach (var entity in _world.View().Include<Player, LocalTransform>())
			{
				var clientId = players.Get(entity).ClientId;
				var input = _inputs.Get<MoveInput>(clientId).FadeOut(_inputFadeOut);

				transforms.Get(entity).Position += input.Direction * deltaTime * _speed;
			}
		}
	}
}
