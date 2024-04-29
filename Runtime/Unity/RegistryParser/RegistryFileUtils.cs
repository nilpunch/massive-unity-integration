using System.IO;
using Massive;
using Massive.Serialization;

namespace UPR
{
	public static class RegistryFileUtils
	{
		public static void WriteToFile(string fileName, IRegistry registry, IRegistryParser registryParser)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(fileName)!);

			using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
			{
				registryParser.Write(registry, stream);
			}
		}

		public static IRegistry ReadFromFile(string fileName, IRegistryParser registryParser)
		{
			var registry = new Registry();

			using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
			{
				registryParser.Read(registry, stream);
			}

			return registry;
		}
	}
}
