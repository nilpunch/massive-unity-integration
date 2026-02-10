using System.Collections.Generic;
using Massive.QoL;
using UnityEngine;

namespace Massive.Unity
{
	[CreateAssetMenu(menuName = "Massive/View DB", fileName = "MyConfig")]
	public class ViewDataBase : ScriptableConfig<ViewDataBase>
	{
		[field: SerializeField] public List<EntityView> ViewsPrefabs { get; private set; }

		public ViewAsset GetViewAsset(EntityView prefab)
		{
			return new ViewAsset(ViewsPrefabs.IndexOf(prefab));
		}

		public EntityView GetViewPrefab(ViewAsset viewAsset)
		{
			return ViewsPrefabs[viewAsset.Id];
		}
	}
}
