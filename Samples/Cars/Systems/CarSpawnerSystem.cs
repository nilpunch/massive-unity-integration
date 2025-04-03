using Mathematics.Fixed;
using UnityEngine;

namespace Massive.Unity.Samples.Cars
{
	public class CarSpawnerSystem : UpdateSystem
	{
		[SerializeField] private EntityView _carViewPrefab;
		[SerializeField] private ViewDataBase _viewDataBase;
		[SerializeField] private Car _carSettings;
		[SerializeField] private int _carsLimit = 100;

		private World _world;

		public override void Init(ServiceLocator serviceLocator)
		{
			_world = serviceLocator.Find<World>();
		}

		public override void UpdateFrame(FP deltaTime)
		{
			if (_world.SparseSet<Car>().Count == _carsLimit)
			{
				return;
			}

			var carId = _world.Create(_carSettings);
			float factor = (float)_world.SparseSet<Car>().Count / _carsLimit;

			var carTransform = new LocalTransform();
			carTransform.Scale = Vector3.one;
			carTransform.Position = (Vector2.one * factor * 10f).FromXZ();
			carTransform.Rotation = Quaternion.AngleAxis(factor * 360f, Vector3.up);
			_world.Set(carId, carTransform);
		}
	}
}
