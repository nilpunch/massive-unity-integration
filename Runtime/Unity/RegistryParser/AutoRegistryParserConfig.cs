using Massive.Serialization;
using UnityEngine;

namespace Massive.Unity
{
	[CreateAssetMenu(fileName = "AutoRegistryParserConfig", menuName = "Massive ECS/Auto Registry Parser Config")]
	public class AutoRegistryParserConfig : RegistryParserConfig
	{
		public override IRegistrySerializer CreateParser()
		{
			var registryParser = new RegistrySerializer();

			foreach (var monoComponentReflection in ComponentReflectors.All)
			{
				monoComponentReflection.PopulateRegistryParser(registryParser);
			}

			return registryParser;
		}
	}
}
