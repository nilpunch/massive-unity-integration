#if UNITY_EDITOR
using UnityEditor;

namespace Massive.Unity.Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(EntityProvider))]
	internal class EntityProviderEditor : UnityEditor.Editor
	{
	}
}
#endif
