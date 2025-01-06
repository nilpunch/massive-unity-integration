using Massive.Netcode;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public class CharactersRotator : UpdateSystem
	{
		[SerializeField] private float _rotation = 400f;

		private Registry _registry;

		public override void Init(Simulation simulation)
		{
			_registry = simulation.Registry;
		}

		public override void UpdateFrame(float deltaTime)
		{
			_registry.View().Include<WeaponState>().ForEachExtra((deltaTime, _rotation),
				(int id, ref LocalTransform characterTransform, (float DeltaTime, float RotationSpeed) args) =>
			{
				characterTransform.Rotation *= Quaternion.AngleAxis(args.RotationSpeed * args.DeltaTime, Vector3.forward);
			});
		}
	}
}
