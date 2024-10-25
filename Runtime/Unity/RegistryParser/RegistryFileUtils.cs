using System.IO;
using Massive.Serialization;

namespace Massive.Unity
{
	public static class RegistryFileUtils
	{
		public static void WriteToFile(string fileName, Registry registry, IRegistrySerializer registrySerializer)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(fileName)!);

			using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
			{
				registrySerializer.Serialize(registry, stream);
			}
		}

		public static void ReadFromFile(string fileName, Registry registry, IRegistrySerializer registrySerializer)
		{
			using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				registrySerializer.Deserialize(registry, stream);
			}
		}
	}
}
