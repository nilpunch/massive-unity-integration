using System.Collections.Generic;
using UnityEngine;

namespace Massive.Unity
{
	[CreateAssetMenu]
	public class ViewDataBaseConfig : ScriptableObject
	{
		[field: SerializeField] public List<GameObject> ViewsPrefabs { get; private set; }

		public ViewAsset GetAssetId(GameObject prefab)
		{
			return new ViewAsset(ViewsPrefabs.IndexOf(prefab));
		}

		public GameObject GetViewPrefab(ViewAsset viewAsset)
		{
			return ViewsPrefabs[viewAsset.Id];
		}
	}
}