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

		public override void OnEntityAssigned(Registry registry, Entity entity)
		{
			_rootObject.SetActive(true);
		}

		public override void OnEntityUnassigned()
		{
			_rootObject.SetActive(false);
		}
	}
}
