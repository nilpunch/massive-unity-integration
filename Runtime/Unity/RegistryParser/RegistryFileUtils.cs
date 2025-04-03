using System.IO;
using Massive.Serialization;

namespace Massive.Unity
{
	public static class RegistryFileUtils
	{
		public static void WriteToFile(string fileName, World world, IRegistrySerializer registrySerializer)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(fileName)!);

			using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
			{
				registrySerializer.Serialize(world, stream);
			}
		}

		public static void ReadFromFile(string fileName, World world, IRegistrySerializer registrySerializer)
		{
			using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				registrySerializer.Deserialize(world, stream);
			}
		}
	}
}
