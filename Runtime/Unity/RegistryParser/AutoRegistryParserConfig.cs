using Massive.Serialization;
using UnityEngine;

namespace UPR
{
	[CreateAssetMenu(fileName = "AutoRegistryParserConfig", menuName = "Massive ECS/Auto Registry Parser Config")]
	public class AutoRegistryParserConfig : RegistryParserConfig
	{
		public override IRegistryParser CreateParser()
		{
			var registryParser = new RegistryParser();

			foreach (var monoComponentReflection in ComponentReflectors.All)
			{
				monoComponentReflection.PopulateRegistryParser(registryParser);
			}

			return registryParser;
		}
	}
}
