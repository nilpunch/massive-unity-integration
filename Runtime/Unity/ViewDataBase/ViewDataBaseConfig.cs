using System.Collections.Generic;
using UnityEngine;

namespace Massive.Unity
{
	[CreateAssetMenu]
	public class ViewDataBaseConfig : ScriptableObject
	{
		[field: SerializeField] public List<EntityView> ViewsPrefabs { get; private set; }

		public ViewAsset GetAssetId(EntityView prefab)
		{
			return new ViewAsset(ViewsPrefabs.IndexOf(prefab));
		}

		public EntityView GetViewPrefab(ViewAsset viewAsset)
		{
			return ViewsPrefabs[viewAsset.Id];
		}
	}
}