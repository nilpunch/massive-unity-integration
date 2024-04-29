using Massive.Serialization;
using UnityEngine;

namespace Massive.Unity
{
	public abstract class RegistryParserConfig : ScriptableObject
	{
		public abstract IRegistryParser CreateParser();
	}
}
