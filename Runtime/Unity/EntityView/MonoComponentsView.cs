﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Massive.Unity
{
	public class MonoComponentsView : MonoBehaviour
	{
		public Registry Registry { get; protected set; }
		public Entity Entity  { get; protected set; }

		[SerializeReference] public List<object> DummyComponents;
	}
}