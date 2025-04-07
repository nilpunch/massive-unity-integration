using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public class CharactersRotator : UpdateSystem
	{
		[SerializeField] private float _rotation = 400f;

		private World _world;

		public override void Init(ServiceLocator serviceLocator)
		{
			_world = serviceLocator.Find<World>();
		}

		public override void UpdateFrame(float deltaTime)
		{
			_world.View().Include<Weapon>().ForEachExtra((deltaTime, _rotation),
				(int id, ref LocalTransform characterTransform, (float DeltaTime, float RotationSpeed) args) =>
			{
				characterTransform.Rotation *= Quaternion.AngleAxis(args.RotationSpeed * args.DeltaTime, Vector3.forward);
			});
		}
	}
}
