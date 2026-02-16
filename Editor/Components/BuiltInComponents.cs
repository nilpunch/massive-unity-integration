using System;
using UnityEditor;

#if UNITY_EDITOR
namespace Massive.Unity.Editor
{
	[InitializeOnLoad]
	public static class BuiltInComponents
	{
		static BuiltInComponents()
		{
			TryAddComponent("Massive.QoL.ViewAsset, Massive.QoL");
		}

		private static void TryAddComponent(string typeName)
		{
			var type = Type.GetType(typeName);

			if (type != null)
			{
				InspectorComponents.Add(type);
			}
		}
	}
}
#endif