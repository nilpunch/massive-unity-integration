namespace Massive.Unity.Samples.Shooter
{
	public class BulletStateComponent : UnmanagedComponent<BulletState, BulletStateComponent>
	{
	}

	public enum WeaponStatus
	{
		None,
		Check,
		Ready,
	}
}
