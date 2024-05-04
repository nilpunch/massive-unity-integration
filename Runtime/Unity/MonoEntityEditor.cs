#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Massive.Unity
{
	[CustomEditor(typeof(MonoEntity))]
	[CanEditMultipleObjects]
	public class MonoEntityEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			GUILayout.Space(5f);

			if (Application.isPlaying)
			{
				if (GUILayout.Button("Destroy Entity"))
				{
					foreach (MonoEntity monoEntity in targets)
					{
						monoEntity.DestroyEntity();
					}
				}
			}
		}
	}
}
#endif
