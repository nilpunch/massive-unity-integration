using Massive.Physics;
using Mathematics.Fixed;
using UnityEngine;

namespace Massive.Unity.Samples.Physics
{
	public class DistanceConstaintProvider : ComponentProvider
	{
		[SerializeField] private EntityProvider _body;
		[SerializeField] private EntityProvider _otherBody;
		[SerializeField] private FVector3 _attachPoint;
		[SerializeField] private FVector3 _otherAttachPoint;
		[SerializeField] private FP _distance = FP.One;
		[SerializeField] private FP _complience = 0.1f.ToFP();

		public override void ApplyToEntity(ServiceLocator serviceLocator, Entity entity)
		{
			serviceLocator.Find<Registry>().Assign(entity, new DistanceConstraint()
			{
				Body = _body.Entity,
				OtherBody = _otherBody.Entity,
				AttachPoint = _attachPoint,
				OtherAttachPoint = _otherAttachPoint,
				Distance = _distance,
				Complience = _complience,
			});
		}
	}
}
