using Massive.Netcode;
using UnityEngine;

namespace Massive.Unity.Samples.Cars
{
	public class CarSpawnerSystem : UpdateSystem
	{
		[SerializeField] private EntityView _carViewPrefab;
		[SerializeField] private ViewDataBase _viewDataBase;
		[SerializeField] private Car _carSettings;
		[SerializeField] private int _carsLimit = 100;

		private Registry _registry;

		public override void Init(Simulation simulation)
		{
			_registry = simulation.Registry;
		}

		public override void UpdateFrame(float deltaTime)
		{
			if (_registry.Set<Car>().Count == _carsLimit)
			{
				return;
			}

			var carId = _registry.Create(_carSettings);
			float factor = (float)_registry.Set<Car>().Count / _carsLimit;

			var carTransform = new LocalTransform();
			carTransform.Scale = Vector3.one;
			carTransform.Position = (Vector2.one * factor * 10f).FromXZ();
			carTransform.Rotation = Quaternion.AngleAxis(factor * 360f, Vector3.up);
			_registry.Assign(carId, carTransform);
		}
	}
}
