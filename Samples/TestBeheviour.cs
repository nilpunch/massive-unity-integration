using UnityEngine;

namespace Massive.Unity.Samples.Farm
{
	public class TestBeheviour : MonoBehaviour
	{
		[SerializeField] private Color _color;
		[SerializeReference, ComponentSelector] private object _component;
	}
}
