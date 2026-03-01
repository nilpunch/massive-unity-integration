using System.Collections.Generic;
using UnityEngine;

namespace Massive.Unity
{
	public class EntityProviderFromPrefab : EntityProviderBase
	{
		[Space]
		[SerializeField] private EntityProvider _entityPrefab;
		[Space]
		[SerializeField] private EntityCreation _create;
		[SerializeField] private CreatedAction _whenCreated;

		private readonly List<object> _dummy = new List<object>();

		public override List<object> Components => _entityPrefab ? _entityPrefab.Components : _dummy;
		public override EntityCreation EntityCreation => _create;
		public override CreatedAction CreatedAction => _whenCreated;
	}
}
