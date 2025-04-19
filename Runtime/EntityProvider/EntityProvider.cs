using System.Collections.Generic;
using UnityEngine;

namespace Massive.Unity
{
	[DisallowMultipleComponent]
	public class EntityProvider : BaseEntityProvider
	{
		[SerializeReference, ComponentPeeker] private List<object> _components = new List<object>();
	}
}
