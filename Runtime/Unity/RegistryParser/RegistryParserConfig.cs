using Massive.Serialization;
using UnityEngine;

namespace UPR
{
	public abstract class RegistryParserConfig : ScriptableObject
	{
		public abstract IRegistryParser CreateParser();
	}
}
