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
			registrySerializer.AddComponent<LocalTransform>();
			registrySerializer.AddComponent<int>();
			registrySerializer.AddComponent<Vector3>();
			registrySerializer.AddComponent<float>();

			registrySerializer.AddNonOwningGroup<Include<int, float>>();

			return registrySerializer;
		}
	}
}
