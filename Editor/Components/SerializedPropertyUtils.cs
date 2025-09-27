#if UNITY_EDITOR
using System;
using UnityEditor;

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
	}
}
#endif