using Massive.Unity;
using UnityEngine;

namespace Massive.Samples.Shooter
{
	public class BulletsUpdater : UpdateSystem
	{
		private IRegistry _registry;
		private View<BulletState, LocalTransform> _bullets;

		public override void Init(IRegistry registry)
		{
			_registry = registry;
			_bullets = registry.View<BulletState, LocalTransform>();
		}

		public override unsafe void UpdateFrame(float deltaTime)
		{
			// foreach (var (entityId, bullet, bulletTransform) in _bullets)
			// {
			// 	bullet->Lifetime -= deltaTime;
			// 	if (bullet->IsDestroyed)
			// 	{
			// 		_registry.Destroy(entityId);
			// 		continue;
			// 	}
			//
			// 	bulletTransform->Position += bullet->Velocity * deltaTime;
			// }

			_bullets.ForEachExtra((_registry, deltaTime), (int entityId, ref BulletState bullet, ref LocalTransform bulletTransform,
				(IRegistry Registry, float DeltaTime) inner) =>
			{
				bullet.Lifetime -= inner.DeltaTime;
				if (bullet.IsDestroyed)
				{
					inner.Registry.Destroy(entityId);
					return;
				}
			
				bulletTransform.Position += bullet.Velocity * inner.DeltaTime;
			});
		}
		
		private void OnGUI()
		{
			float fontScaling = Screen.height / (float)1080;
			
			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));

			GUILayout.BeginVertical();

			GUILayout.FlexibleSpace();

			GUILayout.TextField($"{_registry.Any<BulletState>().Count} Bullets",
				new GUIStyle() { fontSize = Mathf.RoundToInt(70 * fontScaling), normal = new GUIStyleState() { textColor = Color.white } });

			GUILayout.EndVertical();

			GUILayout.EndArea();
		}
	}
}
