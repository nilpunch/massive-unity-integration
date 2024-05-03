using Massive.Unity;
using UnityEngine;

namespace Massive.Samples.Shooter
{
	public class CharactersWeaponator : UpdateSystem
	{
		[SerializeField] private EntityView _bulletViewPrefab;
		[SerializeField] private ViewDataBaseConfig _viewDataBase;
		[SerializeField] private float _cooldown = 0.2f;
		[SerializeField] private float _bulletVelocity = 1f;
		[SerializeField] private float _bulletDamage = 1f;
		[SerializeField] private float _bulletLifetime = 2f;

		private IRegistry _registry;
		private GroupView<WeaponState, LocalTransform> _characters;

		public override void Init(IRegistry registry)
		{
			_registry = registry;
			_characters = new GroupView<WeaponState, LocalTransform>(registry, registry.Group(registry.Many<WeaponState>(), registry.Many<LocalTransform>()));
		}

		public override void UpdateFrame(float deltaTime)
		{
			var args = new Args()
			{
				Registry = _registry,
				DeltaTime = deltaTime,
				BulletViewPrefab = _bulletViewPrefab,
				ViewDataBase = _viewDataBase,
				Cooldown = _cooldown,
				BulletVelocity = _bulletVelocity,
				BulletDamage = _bulletDamage,
				BulletLifetime = _bulletLifetime
			};

			_characters.ForEachExtra(args, (int entity, ref WeaponState weaponState, ref LocalTransform characterTransform, Args args) =>
			{
				weaponState.Cooldown -= args.DeltaTime;
				if (weaponState.Cooldown > 0)
				{
					return;
				}

				weaponState.Cooldown = args.Cooldown;

				var bulletId = args.Registry.Create(new BulletState
				{
					Velocity = characterTransform.Rotation * Vector3.up * args.BulletVelocity,
					Lifetime = args.BulletLifetime,
					Damage = args.BulletDamage
				});

				args.Registry.Assign(bulletId, args.ViewDataBase.GetAssetId(args.BulletViewPrefab));
				args.Registry.Assign(bulletId, characterTransform);
			});
		}

		private struct Args
		{
			public IRegistry Registry;
			public float DeltaTime;
			public EntityView BulletViewPrefab;
			public ViewDataBaseConfig ViewDataBase;
			public float Cooldown;
			public float BulletVelocity;
			public float BulletDamage;
			public float BulletLifetime;
		}
	}
}
