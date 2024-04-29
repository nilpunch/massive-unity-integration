using Massive.Serialization;
using UnityEngine;

namespace Massive.Unity
{
	[CreateAssetMenu]
	public class SampleRegistryParserConfig : RegistryParserConfig
	{
		public override IRegistryParser CreateParser()
		{
			var registryParser = new RegistryParser();
			registryParser.AddComponent<Transform>();
			registryParser.AddComponent<int>();
			registryParser.AddComponent<Vector3>();
			registryParser.AddComponent<float>();

			registryParser.AddNonOwningGroup(new ComponentsGroup<int, float>());

			return registryParser;
		}
	}
}