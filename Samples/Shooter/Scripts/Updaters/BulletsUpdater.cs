﻿using Mathematics.Fixed;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public class BulletsUpdater : UpdateSystem
	{
		private Registry _registry;

		public override void Init(ServiceLocator serviceLocator)
		{
			_registry = serviceLocator.Find<Registry>();
		}

		public override void UpdateFrame(FP deltaTime)
		{
			_registry.View().ForEachExtra((_registry, deltaTime),
				(int entityId, ref BulletState bullet, ref LocalTransform bulletTransform, (Registry Registry, FP DeltaTime) args) =>
				{
					bullet.Lifetime -= args.DeltaTime.ToFloat();
					if (bullet.IsDestroyed)
					{
						args.Registry.Destroy(entityId);
						return;
					}

					bulletTransform.Position += bullet.Velocity * args.DeltaTime.ToFloat();
				});
		}

		private void OnGUI()
		{
			float fontScaling = Screen.height / (float)1080;

			GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));

			GUILayout.BeginVertical();

			GUILayout.FlexibleSpace();

			GUILayout.TextField($"{_registry.View().Include<BulletState>().Count()} Bullets",
				new GUIStyle() { fontSize = Mathf.RoundToInt(70 * fontScaling), normal = new GUIStyleState() { textColor = Color.white } });

			GUILayout.EndVertical();

			GUILayout.EndArea();
		}
	}
}
