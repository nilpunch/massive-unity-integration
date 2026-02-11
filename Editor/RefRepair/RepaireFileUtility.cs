#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity.Editor
{
    internal static class RepaireFileUtility
    {
        private const string REFLINE_PATTERN = "- rid:";
        public static void Replace(string[] fileLines, string oldTypeData, string newTypeData)
        {
            for (int i = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].Contains(REFLINE_PATTERN))
                {
                    fileLines[++i].Replace(oldTypeData, newTypeData);
                }
            }
        }
        public static int NextRefLine(string[] fileLines, int startIndex)
        {
            for (int i = startIndex; i < fileLines.Length; i++)
            {
                if (fileLines[i].Contains(REFLINE_PATTERN))
                {
                    return ++i;
                }
            }
            return -1;
        }
        public static string GenerateReplacedLine(TypeData typeData)
        {
            return $"type: {{class: {typeData.ClassName}, ns: {typeData.NamespaceName}, asm: {typeData.AssemblyName}}}";
        }
        public static void RepairAsset(MissingRefContainer container)
        {
            if (container.IsEmpty) { return; }

            UnityObjectDataBase unityObjectData = null;
            FileScope fileScope = default;
            for (int i = 0; i < container.collectedMissingTypesBufferCount; i++)
            {
                ref var missing = ref container.collectedMissingTypesBuffer[i];

                if (missing.ResolvingData.IsEmpty)
                {
                    continue;
                }

                if (unityObjectData != missing.UnityObject)
                {
                    unityObjectData = missing.UnityObject;
                    fileScope.Dispose();
                    fileScope = new FileScope(unityObjectData.GetLocalAssetPath());
                }

                int lineIndex = NextRefLine(fileScope.lines, 0);
                while (lineIndex > 0)
                {
                    var line = fileScope.lines[lineIndex];

                    line = line.Replace(missing.ResolvingData.OldSerializedInfoLine, missing.ResolvingData.NewSerializedInfoLine);
                    bool isChanged = !ReferenceEquals(fileScope.lines[lineIndex], line);

                    if (isChanged)
                    {
                        fileScope.WasChanged = true;
                        fileScope.lines[lineIndex] = line;
                        break;
                    }
                    lineIndex = NextRefLine(fileScope.lines, lineIndex);
                }
            }
            fileScope.Dispose();

            container.RemoveResolved();
        }
        public struct FileScope : IDisposable
        {
            public readonly string FilePath;
            public readonly string LocalAssetPath;
            public bool WasChanged;
            public string[] lines;

            public FileScope(string localAssetPath)
            {
                LocalAssetPath = localAssetPath;
                FilePath = $"{Application.dataPath.Replace("/Assets", "")}/{localAssetPath}";
                lines = File.ReadAllLines(localAssetPath);
                WasChanged = false;
            }

            public void Dispose()
            {
                if (string.IsNullOrEmpty(FilePath) || !WasChanged)
                {
                    return;
                }
                File.WriteAllLines(FilePath, lines);
                AssetDatabase.ImportAsset(LocalAssetPath, ImportAssetOptions.ForceUpdate);
                AssetDatabase.Refresh();
                WasChanged = false;
            }
        }
    }
}
#endif