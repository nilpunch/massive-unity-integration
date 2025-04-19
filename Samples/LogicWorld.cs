namespace Massive.Unity.Samples
{
	[WorldType]
	public struct Logic
	{
	}

	public abstract class LogicWorld : World<Logic>
	{
	}

	[WorldType]
	public struct View
	{
	}

	public abstract class ViewWorld : World<View>
	{
	}
}
