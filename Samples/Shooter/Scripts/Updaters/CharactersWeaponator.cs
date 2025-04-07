using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public class CharactersWeaponator : UpdateSystem
	{
		[SerializeField] private EntityView _bulletViewPrefab;
		[SerializeField] private float _cooldown = 0.2f;
		[SerializeField] private float _bulletVelocity = 1f;
		[SerializeField] private float _bulletDamage = 1f;
		[SerializeField] private float _bulletLifetime = 2f;
		[SerializeField] private Vector3 _bulletScale = Vector3.one * 2f;

		private World _world;

		public override void Init(ServiceLocator serviceLocator)
		{
			_world = serviceLocator.Find<World>();
		}

		public override void UpdateFrame(float deltaTime)
		{
			var weapons = _world.DataSet<Weapon>();
			var transforms = _world.DataSet<LocalTransform>();
			
			foreach (var entityId in _world.View().Filter<Include<Weapon, LocalTransform>>())
			{
				ref var weaponState = ref weapons.Get(entityId);
				ref var characterTransform = ref transforms.Get(entityId);

				weaponState.Cooldown -= deltaTime;
				if (weaponState.Cooldown > 0f)
				{
					continue;
				}

				weaponState.Cooldown = _cooldown;

				var bulletId = _world.Create(new BulletState
				{
					Velocity = characterTransform.Rotation * Vector3.up * _bulletVelocity,
					Lifetime = _bulletLifetime,
					Damage = _bulletDamage
				});

				// _registry.Assign(bulletId, _bulletViewPrefab.ViewAsset);
				var bulletTransform = characterTransform;
				bulletTransform.Scale = _bulletScale;
				_world.Set(bulletId, bulletTransform);
			}
		}
	}
}
