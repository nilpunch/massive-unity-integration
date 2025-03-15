using Mathematics.Fixed;
using UnityEngine;

namespace Massive.Unity.Samples.Physics
{
	public class ChainMakerSystem : UpdateSystem
	{
		[SerializeField] private FP _radius = FP.Half;
		[SerializeField] private FP _density = FP.One;
		[SerializeField] private FP _complience = FP.One;
		[SerializeField] private int _amount = 20;
		[SerializeField] private EntityView _viewPrefab;
		
		public override void Init(ServiceLocator serviceLocator)
		{
			// var registry = serviceLocator.Find<Registry>();
			//
			// var viewAsset = serviceLocator.Find<ViewDataBase>().GetViewAsset(_viewPrefab);
			//
			// var mass = (4f / 3f).ToFP() * FP.Pi * _radius * _radius * _radius * _density;
			// var invMass = FP.One / mass;
			// var inertia = 2.ToFP() / 5.ToFP() * mass * _radius * _radius;
			// var position = new FVector3(
			// 	transform.position.x.ToFP(),
			// 	transform.position.y.ToFP(),
			// 	transform.position.z.ToFP());
			// var rotation = new FQuaternion(
			// 	transform.rotation.x.ToFP(),
			// 	transform.rotation.y.ToFP(),
			// 	transform.rotation.z.ToFP(),
			// 	transform.rotation.w.ToFP());
			//
			// var rootEntity = registry.CreateEntity();
			// registry.Assign(rootEntity, viewAsset);
			// registry.Assign(rootEntity, new Body()
			// {
			// 	Position = position,
			// 	PrevPosition = position,
			// 	Rotation = rotation,
			// 	PrevRotation = rotation,
			// 	InvMass = FP.Zero,
			// 	InvInertiaTensor = FVector3.Zero
			// });
			//
			// for (int i = 0; i < _amount; i++)
			// {
			// 	position += FVector3.Right * _radius * FP.Two;
			// 	
			// 	var entity = registry.CreateEntity();
			// 	registry.Assign(entity, new Body()
			// 	{
			// 		Position = position,
			// 		PrevPosition = position,
			// 		Rotation = rotation,
			// 		PrevRotation = rotation,
			// 		Velocity = FVector3.Zero,
			// 		AngularVelocity = FVector3.Zero,
			// 		InvMass = invMass,
			// 		InvInertiaTensor = new FVector3(FP.One / inertia, FP.One / inertia, FP.One / inertia)
			// 	});
			// 	registry.Assign(entity, viewAsset);
			// 	registry.Assign(entity, new LocalTransform() { Rotation = Quaternion.identity, Scale = Vector3.one });
			//
			// 	var constraintEntity = registry.CreateEntity();
			// 	if (i == 0)
			// 	{
			// 		registry.Assign(constraintEntity, new DistanceConstraint()
			// 		{
			// 			Body = rootEntity,
			// 			OtherBody = entity,
			// 			AttachPoint = FVector3.Zero,
			// 			OtherAttachPoint = FVector3.Left * _radius,
			// 			Distance = _radius,
			// 			Complience = _complience,
			// 		});
			// 	}
			// 	else
			// 	{
			// 		registry.Assign(constraintEntity, new DistanceConstraint()
			// 		{
			// 			Body = rootEntity,
			// 			OtherBody = entity,
			// 			AttachPoint = FVector3.Right * _radius,
			// 			OtherAttachPoint = FVector3.Left * _radius,
			// 			Distance = FP.Zero,
			// 			Complience = _complience,
			// 		});
			// 	}
			// 	rootEntity = entity;
			// }
		}

		public override void UpdateFrame(FP deltaTime)
		{
			
		}
	}
}
