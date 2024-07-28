using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public class CharactersRotator : UpdateSystem
	{
		[SerializeField] private float _rotation = 400f;

		private IRegistry _registry;

		public override void Init(IRegistry registry)
		{
			_registry = registry;
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
