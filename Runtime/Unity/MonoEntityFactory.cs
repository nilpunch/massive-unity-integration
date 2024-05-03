using UnityEngine;

namespace Massive.Unity
{
	public class MonoEntityFactory : IFactory<MonoEntity>
	{
		private int _entityCounter;

		public MonoEntity Create()
		{
			return new GameObject($"Entity {++_entityCounter}").AddComponent<MonoEntity>();
		}
	}
}
