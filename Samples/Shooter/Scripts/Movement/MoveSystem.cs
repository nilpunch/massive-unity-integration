using Massive.Netcode;
using Mathematics.Fixed;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public class MoveSystem : UpdateSystem
	{
		[SerializeField] private float _speed = 5f;
		[SerializeField] private FadeOutConfig _inputFadeOut;

		private Inputs _inputs;
		private Registry _registry;

		public override void Init(ServiceLocator serviceLocator)
		{
			_registry = serviceLocator.Find<Registry>();
			_inputs = serviceLocator.Find<Inputs>();
		}

		public override void UpdateFrame(FP deltaTime)
		{
			var transforms = _registry.DataSet<LocalTransform>();
			var players = _registry.DataSet<Player>();
			foreach (var entity in _registry.View().Include<Player, LocalTransform>())
			{
				var clientId = players.Get(entity).ClientId;
				var input = _inputs.Get<MoveInput>(clientId).FadeOut(_inputFadeOut);

				transforms.Get(entity).Position += input.Direction * deltaTime.ToFloat() * _speed;
			}
		}
	}
}
