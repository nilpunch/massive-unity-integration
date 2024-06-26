﻿using Massive.Unity;
using UnityEngine;

namespace Massive.Samples.Shooter
{
	public class CharactersRotator : UpdateSystem
	{
		[SerializeField] private float _rotation = 400f;

		private FilterView<LocalTransform> _characters;

		public override void Init(IRegistry registry)
		{
			_characters = registry.FilterView<LocalTransform>(registry.Filter<Include<WeaponState>>());
		}

		public override void UpdateFrame(float deltaTime)
		{
			_characters.ForEachExtra((deltaTime, _rotation), (int id, ref LocalTransform characterTransform, (float deltaTime, float rotationSpeed) inner) =>
			{
				characterTransform.Rotation *= Quaternion.AngleAxis(inner.rotationSpeed * inner.deltaTime, Vector3.forward);
			});
		}
	}
}
