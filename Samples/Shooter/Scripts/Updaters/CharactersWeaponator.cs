using Massive.Netcode;
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

		private Registry _registry;

		public override void Init(Simulation simulation)
		{
			_registry = simulation.Registry;
		}

		public override void UpdateFrame(float deltaTime)
		{
			var weapons = _registry.DataSet<WeaponState>();
			var transforms = _registry.DataSet<LocalTransform>();
			
			foreach (var entityId in _registry.View().Filter<Include<WeaponState, LocalTransform>>())
			{
				ref var weaponState = ref weapons.Get(entityId);
				ref var characterTransform = ref transforms.Get(entityId);

				weaponState.Cooldown -= deltaTime;
				if (weaponState.Cooldown > 0)
				{
					continue;
				}

				weaponState.Cooldown = _cooldown;

				var bulletId = _registry.Create(new BulletState
				{
					Velocity = characterTransform.Rotation * Vector3.up * _bulletVelocity,
					Lifetime = _bulletLifetime,
					Damage = _bulletDamage
				});

				// _registry.Assign(bulletId, _bulletViewPrefab.ViewAsset);
				var bulletTransform = characterTransform;
				bulletTransform.Scale = _bulletScale;
				_registry.Assign(bulletId, bulletTransform);
			}
		}
	}
}
