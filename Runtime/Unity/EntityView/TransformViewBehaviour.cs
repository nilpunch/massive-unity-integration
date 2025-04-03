using UnityEngine;

namespace Massive.Unity
{
	public class TransformViewBehaviour : ViewBehaviour
	{
		[SerializeField] private Transform _rootTransform;

		private DataSet<LocalTransform> _localTransforms;
		private Entity _entity;

		public override void OnEntityAssigned(World world, Entity entity)
		{
			_entity = entity;
			_localTransforms = world.DataSet<LocalTransform>();
		}

		public override void OnEntityRemoved()
		{
			_localTransforms = null;
			_entity = Entity.Dead;
		}

		private void LateUpdate()
		{
			if (!_localTransforms.Has(_entity.Id))
			{
				return;
			}

			var localTransformData = _localTransforms.Get(_entity.Id);

			_rootTransform.localPosition = localTransformData.Position;
			_rootTransform.localRotation = localTransformData.Rotation;
			_rootTransform.localScale = localTransformData.Scale;
		}
	}
}
