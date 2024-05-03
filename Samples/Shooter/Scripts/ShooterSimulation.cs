using System.Diagnostics;
using UnityEngine;

namespace Massive.Samples.Shooter
{
	public class ShooterSimulation : MonoBehaviour
	{
		[SerializeField, Min(1)] private int _saveEachNthFrame = 1;
		[SerializeField] private int _resimulationsPerFrame = 120;
		[SerializeField] private int _entitiesCapacity = 3000;
		[SerializeField] private int _charactersAmount = 10;

		[Header("Entities")] [SerializeField] private EntityRoot<CharacterState> _characterPrefab;
		[SerializeField] private EntityRoot<BulletState> _bulletPrefab;

		private MassiveRegistry _registry;
		private WorldUpdater[] _worldUpdaters;

		private EntitySynchronisation<CharacterState> _characterSynchronisation;
		private EntitySynchronisation<BulletState> _bulletSynchronisation;

		private void Awake()
		{
			_registry = new MassiveRegistry(dataCapacity: _entitiesCapacity, framesCapacity: _resimulationsPerFrame + 1);

			_worldUpdaters = FindObjectsOfType<WorldUpdater>();
			foreach (var worldUpdaters in _worldUpdaters)
			{
				worldUpdaters.Init(_registry);
			}

			_characterSynchronisation = new EntitySynchronisation<CharacterState>(new EntityFactory<CharacterState>(_characterPrefab));
			_bulletSynchronisation = new EntitySynchronisation<BulletState>(new EntityFactory<BulletState>(_bulletPrefab));

			for (int i = 0; i < _charactersAmount; i++)
			{
				var characterId = _registry.Create(new CharacterState()
				{
					Transform = new EntityTransform()
					{
						Position = Vector3.right * (i - _charactersAmount / 2f) * 1.5f,
						Rotation = Quaternion.AngleAxis(180f * (i - _charactersAmount / 2f) / _charactersAmount, Vector3.forward)
					}
				});
				_registry.Assign(characterId, new WeaponState());
			}

			_registry.SaveFrame();
		}

		private int _currentFrame;

		private float _elapsedTime;

		private void Update()
		{
			Stopwatch stopwatch = Stopwatch.StartNew();

			if (_registry.CanRollbackFrames >= 0)
			{
				int framesToRollback = _registry.CanRollbackFrames;

				int currentFrameCompressed = _currentFrame / _saveEachNthFrame;
				
				int compressedFramesToRollback = currentFrameCompressed - (_currentFrame - framesToRollback) / _saveEachNthFrame;
				
				_currentFrame = (currentFrameCompressed - compressedFramesToRollback) * _saveEachNthFrame;
				_registry.Rollback(compressedFramesToRollback);
			}

			_elapsedTime += Time.deltaTime;

			float deltaTime = 1f / 60f;

			int targetFrame = Mathf.RoundToInt(_elapsedTime * 60);

			var previousCurrentFrame = _currentFrame;
			
			while (_currentFrame < targetFrame)
			{
				foreach (var worldUpdater in _worldUpdaters)
				{
					worldUpdater.UpdateWorld(deltaTime);
				}

				_currentFrame++;
				if (_currentFrame % _saveEachNthFrame == 0)
				{
					_registry.SaveFrame();
				}
			}

			_characterSynchronisation.Synchronize(_registry.Components<CharacterState>());
			_bulletSynchronisation.Synchronize(_registry.Components<BulletState>());

			_debugTime = stopwatch.ElapsedMilliseconds;
			_debugResimulations = _currentFrame - previousCurrentFrame;
		}

		private long _debugTime;
		private int _debugResimulations;

		private void OnGUI()
		{
			GUILayout.TextField($"{_debugTime}ms Simulation", new GUIStyle() { fontSize = 70, normal = new GUIStyleState() { textColor = Color.white } });
			GUILayout.TextField($"{_debugResimulations} Resimulations",
				new GUIStyle() { fontSize = 50, normal = new GUIStyleState() { textColor = Color.white } });
			GUILayout.TextField($"{_registry.Components<CharacterState>().Count} Characters",
				new GUIStyle() { fontSize = 50, normal = new GUIStyleState() { textColor = Color.white } });
			GUILayout.TextField($"{_registry.Components<BulletState>().Count} Bullets",
				new GUIStyle() { fontSize = 50, normal = new GUIStyleState() { textColor = Color.white } });
		}
	}
}
