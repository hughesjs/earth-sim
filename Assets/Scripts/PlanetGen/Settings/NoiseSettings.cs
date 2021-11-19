using System;
using UnityEngine;

namespace PlanetGen.Settings
{
	[Serializable]
	public class NoiseSettings
	{
		public float strength = 1.0f;
		public float baseRoughness = 1.0f;
		public float roughness = 2.0f;
		public float persistence = 0.5f;
		public float minValue = 0.0f;
		[Range(1,10)] public int numLayers = 1;
		public Vector3 centre;
	}
}
