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

		public static Registry ReadFromFile(string fileName, IRegistrySerializer registrySerializer)
		{
			var registry = new Registry();

			using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				registrySerializer.Deserialize(registry, stream);
			}

			return registry;
		}
	}
}
