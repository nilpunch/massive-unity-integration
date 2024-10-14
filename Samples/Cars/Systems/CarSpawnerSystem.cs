using UnityEngine;

namespace Massive.Unity.Samples.Cars
{
	public class CarSpawnerSystem : RollbackUpdateSystem
	{
		[SerializeField] private EntityView _carViewPrefab;
		[SerializeField] private ViewDataBaseConfig _viewDataBase;
		[SerializeField] private Car _carSettings;
		[SerializeField] private int _carsLimit = 100;

		private Registry _registry;
		private MassiveVariable<Random.State> _randomState;

		public override void Init(Registry registry)
		{
			_registry = registry;
			_randomState = new MassiveVariable<Random.State>(Random.state);
			_randomState.SaveFrame();
		}

		public override void UpdateFrame(float deltaTime)
		{
			if (_registry.Set<Car>().Count == _carsLimit)
			{
				return;
			}

			Random.state = _randomState.Value;

			var carId = _registry.Create(_carSettings);

			var carTransform = new LocalTransform();
			carTransform.Scale = Vector3.one;
			carTransform.Position = (Random.insideUnitCircle * 10f).FromXZ();
			carTransform.Rotation = Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
			_registry.Assign(carId, carTransform);

			_randomState.Value = Random.state;
		}

		public override void SaveFrame()
		{
			_randomState.SaveFrame();
		}

		public override void Rollback(int frames)
		{
			_randomState.Rollback(frames);
		}
	}
}
