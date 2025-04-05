using Mathematics.Fixed;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public class BulletsUpdater : UpdateSystem
	{
		private World _world;

		public override void Init(ServiceLocator serviceLocator)
		{
			_world = serviceLocator.Find<World>();
		}

		public override void UpdateFrame(float deltaTime)
		{
			_world.View().ForEachExtra((_world, deltaTime),
				(int entityId, ref BulletState bullet, ref LocalTransform bulletTransform, (World Registry, float DeltaTime) args) =>
				{
					bullet.Lifetime -= args.DeltaTime;
					if (bullet.IsDestroyed)
					{
						args.Registry.Destroy(entityId);
						return;
					}

					bulletTransform.Position += bullet.Velocity * args.DeltaTime;
				});
		}

		private void OnGUI()
		{
			float fontScaling = Screen.height / (float)1080;

			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));

			GUILayout.BeginVertical();

			GUILayout.FlexibleSpace();

			GUILayout.TextField($"{_world.View().Include<BulletState>().Count()} Bullets",
				new GUIStyle() { fontSize = Mathf.RoundToInt(70 * fontScaling), normal = new GUIStyleState() { textColor = Color.white } });

			GUILayout.EndVertical();

			GUILayout.EndArea();
		}
	}
}
