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
			if (Application.isPlaying)
			{
				GUILayout.Space(5f);

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
