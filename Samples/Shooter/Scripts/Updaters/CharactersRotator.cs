using Mathematics.Fixed;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public class CharactersRotator : UpdateSystem
	{
		[SerializeField] private float _rotation = 400f;

		private Registry _registry;

		public override void Init(ServiceLocator serviceLocator)
		{
			_registry = serviceLocator.Find<Registry>();
		}

		public override void UpdateFrame(FP deltaTime)
		{
			_registry.View().Include<Weapon>().ForEachExtra((deltaTime, _rotation),
				(int id, ref LocalTransform characterTransform, (FP DeltaTime, float RotationSpeed) args) =>
			{
				characterTransform.Rotation *= Quaternion.AngleAxis(args.RotationSpeed * args.DeltaTime.ToFloat(), Vector3.forward);
			});
		}
	}
}
