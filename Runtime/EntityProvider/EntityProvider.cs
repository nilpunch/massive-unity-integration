using System.Collections.Generic;
using Massive.QoL;
using UnityEngine;

namespace Massive.Unity
{
	public enum EntityCreation
	{
		Automatically,
		Manually,
	}

	public enum CreatedAction
	{
		Persist,
		DestroyUnityComponent,
		DestroyGameObject,
	}

	public class EntityProvider : EntityProviderBase
	{
		[SerializeReference, ComponentSelector]
		private List<object> _components = new List<object>();

		[SerializeField] private EntityCreation _create;
		[SerializeField] private CreatedAction _whenCreated;

		public override List<object> Components => _components;
		public override EntityCreation EntityCreation => _create;
		public override CreatedAction CreatedAction => _whenCreated;
	}
}
