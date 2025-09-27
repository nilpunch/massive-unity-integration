using Massive.QoL;
using UnityEngine;

namespace Massive.Unity.Samples.Tetris
{
	public struct Block
	{
		public Vector2Int Offset;
	}

	public struct Tetromino
	{
		public ListHandle<Block> Shape;

		public Vector2Int Position;
	}

	public struct CreateBoxSystem
	{
		private DataSet<Tetromino> _tetrominos;
		private AutoAllocator<Block> _blocks;
		private World _world;
		private Systems _systems;

		public void Initialize(World world)
		{
			_world = world;
			_tetrominos = world.DataSet<Tetromino>();
			_blocks = world.AutoAllocator<Block>();
		}

		public void Update(int createAmount)
		{
			for (int i = 0; i < createAmount; i++)
			{
				var entity = _world.Create();

				var shapeList = _blocks.AllocAutoList(entity, 4);
				shapeList.Add(new Block() { Offset = new Vector2Int(0, 0) });
				shapeList.Add(new Block() { Offset = new Vector2Int(1, 0) });
				shapeList.Add(new Block() { Offset = new Vector2Int(1, 1) });
				shapeList.Add(new Block() { Offset = new Vector2Int(0, 1) });

				_tetrominos.Set(entity, new Tetromino()
				{
					Position = Vector2Int.zero,
					Shape = shapeList,
				});
			}
		}
	}

	public struct FallSystem : IUpdate
	{
		private AutoAllocator<Block> _blocks;
		private DataSet<Tetromino> _tetrominos;

		public World World { get; set; }

		public void Initialize(World world)
		{
			_tetrominos = world.DataSet<Tetromino>();
			_blocks = world.AutoAllocator<Block>();
		}

		public void Update()
		{
			foreach (var id in _tetrominos)
			{
				ref var tetromino = ref _tetrominos.Get(id);

				var shape = tetromino.Shape.In(_blocks);

				var isCollidedWithFloor = false;
				foreach (var block in shape)
				{
					if ((tetromino.Position + block.Offset).y <= 0)
					{
						isCollidedWithFloor = true;
						break;
					}
				}

				if (!isCollidedWithFloor)
				{
					tetromino.Position += Vector2Int.down;
				}
			}
		}
	}

	public struct DestroyTetrominosSystem : IUpdate
	{
		private DataSet<Tetromino> _tetrominos;
		private AutoAllocator<Block> _blocks;

		public void Initialize(World world)
		{
			World = world;
			_tetrominos = world.DataSet<Tetromino>();
			_blocks = world.AutoAllocator<Block>();
		}

		public void Update()
		{
			foreach (var id in _tetrominos)
			{
				// ref var tetromino = ref _tetrominos.Get(id);
				//
				// _blocks.Free(tetromino.Shape);

				World.Destroy(id);
			}
		}

		public World World { get; set; }
	}
}
