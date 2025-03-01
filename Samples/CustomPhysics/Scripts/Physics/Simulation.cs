using Mathematics.Fixed;

namespace Massive.Unity.Samples.Physics
{
	public static class Simulation
	{
		public static void Integrate(Registry registry, FP deltaTime, FVector3 gravity)
		{
			var integration = new Integration(deltaTime, gravity);
			registry.View().ForEach<Integration, Body>(ref integration);
		}

		public static void UpdateVelocities(Registry registry, FP deltaTime)
		{
			var velocityUpdate = new VelocityUpdate(deltaTime);
			registry.View().ForEach<VelocityUpdate, Body>(ref velocityUpdate);
		}

		public static void SolveDistanceConstraints(Registry registry, FP deltaTime)
		{
			var distanceConstraintSolver = new DistanceConstraintSolver(registry.DataSet<Body>(), deltaTime);
			registry.View().ForEach<DistanceConstraintSolver, DistanceConstraint>(ref distanceConstraintSolver);
		}
	}
}
