using UnityEngine;

namespace Massive.Unity
{
	public class ViewPool
	{
		private readonly ViewDataBaseConfig _config;
		private readonly VariantPool<ViewAsset, GameObject> _viewPool = new VariantPool<ViewAsset, GameObject>();
		private readonly DataSet<Transform> _poolRoots = new DataSet<Transform>();

		public ViewPool(ViewDataBaseConfig config)
		{
			_config = config;
		}

		public GameObject GetViewPrefab(ViewAsset viewAsset)
		{
			return _config.GetViewPrefab(viewAsset);
		}

		public GameObject GetView(ViewAsset viewAsset)
		{
			if (!_viewPool.ContainsVariant(viewAsset))
			{
				var viewPrefab = _config.GetViewPrefab(viewAsset);
				var poolRoot = new GameObject(viewPrefab.name + " Pool").transform;
				_viewPool.AddVariant(viewAsset, new Pool<GameObject>(new PrefabFactory<GameObject>(viewPrefab, poolRoot)));
				_poolRoots.Assign(viewAsset.Id, poolRoot);
			}

			var view = _viewPool.Get(viewAsset);
			view.SetActive(true);
			return view;
		}

		public void ReturnView(GameObject view)
		{
			view.SetActive(false);
			view.transform.SetParent(_poolRoots.Get(_viewPool.GetKey(view).Id));
			_viewPool.Return(view);
		}
	}
}