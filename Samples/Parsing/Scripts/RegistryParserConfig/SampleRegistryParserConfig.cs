using Massive.Serialization;
using UnityEngine;

namespace Massive.Unity
{
	[CreateAssetMenu]
	public class SampleRegistryParserConfig : RegistryParserConfig
	{
		public override IRegistrySerializer CreateParser()
		{
			var registrySerializer = new RegistrySerializer();
			return registrySerializer;
		}
	}
}
