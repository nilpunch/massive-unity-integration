using UnityEngine;

namespace Massive.Unity
{
	public class SetActiveWhenAssigned : ViewBehaviour
	{
		[SerializeField] private GameObject _rootObject;

		private void Awake()
		{
			gameObject.SetActive(false);
		}

		public override void OnEntityAssigned(World world, Entity entity)
		{
			_rootObject.SetActive(true);
		}

		public override void OnEntityRemoved()
		{
			_rootObject.SetActive(false);
		}
	}
}
