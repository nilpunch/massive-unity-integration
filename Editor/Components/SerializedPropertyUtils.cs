#if UNITY_EDITOR
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
	public static class SerializedPropertyUtils
	{
		/// <summary>
		/// Gets the parent list or array property given a child element (e.g., from Array.data[x]).
		/// </summary>
		public static SerializedProperty GetOwningList(SerializedProperty elementProperty)
		{
			if (elementProperty == null || string.IsNullOrEmpty(elementProperty.propertyPath))
			{
				return null;
			}

			var path = elementProperty.propertyPath;

			// Look for ".Array.data[" which marks the index access.
			int arrayDataIndex = path.LastIndexOf(".Array.data[", StringComparison.Ordinal);
			if (arrayDataIndex < 0)
			{
				return null;
			}

			// Trim off the ".Array.data[x]" part.
			var arrayPath = path.Substring(0, arrayDataIndex);

			return elementProperty.serializedObject.FindProperty(arrayPath);
		}

		public static bool IsArrayElement(SerializedProperty property)
		{
			return property.propertyPath.Contains(".Array.data[");
		}

		public static bool IsSameProperty(SerializedProperty a, SerializedProperty b)
		{
			if (a == null || b == null)
			{
				return false;
			}

			// Compare if they refer to the same field on the same object.
			return a.propertyPath == b.propertyPath &&
				a.serializedObject.targetObject == b.serializedObject.targetObject;
		}

		public static bool IsTypeMixed(SerializedProperty property)
		{
			var path = property.propertyPath;
			Type firstType = null;
			var initialized = false;

			foreach (var target in property.serializedObject.targetObjects)
			{
				var so = new SerializedObject(target);
				var p = so.FindProperty(path);
				var value = p.managedReferenceValue;

				if (value == null)
				{
					if (initialized && firstType != null)
					{
						return true;
					}

					initialized = true;
					continue;
				}

				var type = value.GetType();

				if (!initialized)
				{
					firstType = type;
					initialized = true;
				}
				else if (type != firstType)
				{
					return true;
				}
			}

			return false;
		}

		public static string GetContainerPath(UnityEngine.Object target)
		{
			if (target == null)
				return null;

			var path = AssetDatabase.GetAssetPath(target);

			if (!string.IsNullOrEmpty(path))
				return path;

			if (target is Component comp)
				return comp.gameObject.scene.path;
			if (target is GameObject go)
				return go.scene.path;

			return null;
		}

		public static void MigrateType(SerializedProperty property, Type newType)
		{
			if (property == null) return;
			if (newType == null) return;

			var assetPath = GetContainerPath(property.serializedObject.targetObject);
			if (string.IsNullOrEmpty(assetPath))
				return;

			// Build the replacement YAML type string
			var typeString =
				$"type: {{class: {newType.Name}, ns: {newType.Namespace ?? ""}, asm: {newType.Assembly.GetName().Name}}}";

			SerializedObject serializedObject = property.serializedObject;

			var fileID = GlobalObjectId.GetGlobalObjectIdSlow(serializedObject.targetObject).targetObjectId;

			var yaml = File.ReadAllText(assetPath);

			var startIndex = yaml.IndexOf(fileID.ToString(), StringComparison.Ordinal);

			var splitPath = property.propertyPath.Split('.');
			var fieldName = splitPath[0];
			startIndex = yaml.IndexOf(fieldName, startIndex, StringComparison.Ordinal);

			var arrayIndex = int.Parse(splitPath[2].Split('[', ']')[1]);

			for (int i = 0; i <= arrayIndex; i++)
			{
				startIndex = yaml.IndexOf("rid:", startIndex, StringComparison.Ordinal);
				startIndex += 5;
			}

			var end = yaml.IndexOf('\n', startIndex);
			var ridLength = end - startIndex;
			var rid = yaml.Substring(startIndex, ridLength);

			startIndex += ridLength;
			startIndex = yaml.IndexOf(rid, startIndex, StringComparison.Ordinal);

			var typeIndex = yaml.IndexOf("type:", startIndex, StringComparison.Ordinal);
			var endOfType = yaml.IndexOf('\n', typeIndex);

			yaml = yaml.Remove(typeIndex, endOfType - typeIndex);
			yaml = yaml.Insert(typeIndex, typeString);
			
			File.WriteAllText(assetPath, yaml);

			AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
			AssetDatabase.SaveAssets();
		}

		// Extract RID from property.managedReferenceId
		private static string GetRID(SerializedProperty property)
		{
			// Unity 2020.2+ exposes managedReferenceId directly
			try
			{
				return property.managedReferenceId.ToString();
			}
			catch
			{
				return null;
			}
		}
	}
}
#endif
