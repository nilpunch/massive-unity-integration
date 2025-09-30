#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	public static class ReferenceRepairTool
	{
		[MenuItem("Assets/Fix Missing Serialize References")]
		public static void FixMissingSerializeReferences()
		{
			var objects = Selection.gameObjects;

			var updateMap = new Dictionary<string, string>();
			var assetsToUpdate = new HashSet<string>();

			foreach (var target in objects.SelectMany(obj => obj.GetComponents<MonoBehaviour>()))
			{
				if (SerializationUtility.HasManagedReferencesWithMissingTypes(target))
				{
					assetsToUpdate.Add(GetContainerPath(target));

					var missingTypes = SerializationUtility.GetManagedReferencesWithMissingTypes(target);
					foreach (var missingType in missingTypes)
					{
						var oldTypeName = MissingToYaml(missingType);

						if (updateMap.ContainsKey(oldTypeName))
						{
							continue;
						}

						TypeReplaceWindow.Show(missingType.namespaceName + '.' + missingType.className, (newType) =>
						{
							if (newType != null)
							{
								updateMap.Add(oldTypeName, TypeToYaml(newType));
							}
						});
					}
				}
			}

			foreach (var assetPath in assetsToUpdate)
			{
				var yaml = File.ReadAllText(assetPath);

				foreach (var (oldType, newType) in updateMap)
				{
					yaml = yaml.Replace(oldType, newType);
				}

				File.WriteAllText(assetPath, yaml);
				AssetDatabase.SaveAssets();
				AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
			}
		}

		public static string TypeToYaml(Type type)
		{
			return $"type: {{class: {type.Name}, ns: {type.Namespace ?? ""}, asm: {type.Assembly.GetName().Name}}}";
		}
		
		public static string MissingToYaml(ManagedReferenceMissingType missingType)
		{
			return $"type: {{class: {missingType.className}, ns: {missingType.namespaceName ?? ""}, asm: {missingType.assemblyName}}}";
		}

		public static string GetContainerPath(UnityEngine.Object target)
		{
			if (target == null)
			{
				return null;
			}

			var path = AssetDatabase.GetAssetPath(target);

			if (!string.IsNullOrEmpty(path))
			{
				return path;
			}

			if (target is Component comp)
			{
				return comp.gameObject.scene.path;
			}

			if (target is GameObject go)
			{
				return go.scene.path;
			}

			return null;
		}
	}
}
#endif