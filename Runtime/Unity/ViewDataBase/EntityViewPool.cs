using UnityEngine;

namespace Massive.Unity
{
	public class EntityViewPool
	{
		private readonly ViewDataBase _config;
		private readonly VariantPool<ViewAsset, EntityView> _viewPool = new VariantPool<ViewAsset, EntityView>();
		private readonly DataSet<Transform> _poolRoots = new DataSet<Transform>();

		public EntityViewPool(ViewDataBase config)
		{
			_config = config;
		}

		public EntityView GetViewPrefab(ViewAsset viewAsset)
		{
			return _config.GetViewPrefab(viewAsset);
		}

		public EntityView CreateView(ViewAsset viewAsset)
		{
			if (!_viewPool.ContainsVariant(viewAsset))
			{
				var viewPrefab = _config.GetViewPrefab(viewAsset);
				var poolRoot = new GameObject(viewPrefab.name + " Pool").transform;
				_viewPool.AddVariant(viewAsset, new Pool<EntityView>(new PrefabFactory<EntityView>(viewPrefab, poolRoot)));
				_poolRoots.Set(viewAsset.Id, poolRoot);
			}

			var view = _viewPool.Get(viewAsset);
			return view;
		}

		public void ReturnView(EntityView view)
		{
			view.transform.SetParent(_poolRoots.Get(_viewPool.GetKey(view).Id));
			_viewPool.Return(view);
		}
	}
}
