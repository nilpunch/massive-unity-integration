#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Massive.Unity
{
	[CustomEditor(typeof(MonoComponent), true)]
	[CanEditMultipleObjects]
	public class MonoComponentEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			GUILayout.Space(5f);
			
			if (Application.isPlaying)
			{
				if (GUILayout.Button("Unassign Component"))
				{
					foreach (MonoComponent monoComponent in targets)
					{
						monoComponent.UnassignComponent();
					}
				}
			}
		}
	}
}
#endif
