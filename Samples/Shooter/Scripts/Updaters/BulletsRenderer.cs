using System.Collections.Generic;
using Massive.Netcode;
using UnityEngine;

namespace Massive.Unity.Samples.Shooter
{
	public class BulletsRenderer : UpdateSystem
	{
		[SerializeField] private Texture _texture;
		[SerializeField] private Material _spriteMaterial;

		private Registry _registry;
		private Mesh _quadMesh;
		private List<Matrix4x4> _matrices;
		private Material _materialCopy;

		public override void Init(Simulation simulation)
		{
			_matrices = new List<Matrix4x4>();
			_registry = simulation.Registry;
			_quadMesh = CreateQuadMesh();
			_materialCopy = Instantiate(_spriteMaterial);
			_materialCopy.mainTexture = _texture;
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
				(ref BulletState _, ref LocalTransform bulletTransform, List<Matrix4x4> matrices) =>
				{
					matrices.Add(Matrix4x4.Translate(bulletTransform.Position));
				});
		
			if (_matrices.Count == 0)
			{
				return;
			}
		
			Graphics.RenderMeshInstanced(new RenderParams(_materialCopy), _quadMesh, 0, _matrices);
		
			_matrices.Clear();
		}

		private Mesh CreateQuadMesh()
		{
			Mesh mesh = new Mesh();
			mesh.vertices = new Vector3[]
			{
				new Vector3(-0.5f, -0.5f, 0),
				new Vector3(0.5f, -0.5f, 0),
				new Vector3(0.5f, 0.5f, 0),
				new Vector3(-0.5f, 0.5f, 0)
			};

			mesh.uv = new Vector2[]
			{
				new Vector2(0, 0),
				new Vector2(1, 0),
				new Vector2(1, 1),
				new Vector2(0, 1)
			};

			mesh.triangles = new int[]
			{
				1, 0, 2,
				3, 2, 0
			};

			mesh.bounds = new Bounds(Vector3.zero, Vector3.one);

			mesh.RecalculateNormals();
			mesh.RecalculateBounds();

			return mesh;
		}
	}
}
