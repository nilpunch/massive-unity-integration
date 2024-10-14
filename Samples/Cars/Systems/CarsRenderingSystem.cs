using System.Collections.Generic;
using UnityEngine;

namespace Massive.Unity.Samples.Cars
{
	public class CarsRenderingSystem : UpdateSystem
	{
		[SerializeField] private Material _material;
		[SerializeField] private Mesh _mesh;
		
		private Registry _registry;
		private List<Matrix4x4> _matrices;
		private Material _materialCopy;

		public override void Init(Registry registry)
		{
			_registry = registry;
			_matrices = new List<Matrix4x4>();
			_materialCopy = Instantiate(_material);
			_materialCopy.enableInstancing = true;
		}
		
		private void OnDestroy()
		{
			Destroy(_materialCopy);
		}

		public override void UpdateFrame(float deltaTime)
		{
		}

		private void Update()
		{
			_registry.View().ForEachExtra(_matrices,
				(ref Car _, ref LocalTransform bulletTransform, List<Matrix4x4> matrices) =>
				{
					matrices.Add(Matrix4x4.TRS(bulletTransform.Position, bulletTransform.Rotation, bulletTransform.Scale));
				});

			if (_matrices.Count == 0)
			{
				return;
			}

			Graphics.RenderMeshInstanced(new RenderParams(_materialCopy), _mesh, 0, _matrices);

			_matrices.Clear();
		}
	}
}
