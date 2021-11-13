using System;
using UnityEngine;

namespace PlanetGen.Settings
{
    [Serializable]
    public class NoiseSettings
    {
        public float strength = 1.0f;
        public float roughness = 1.0f;
        public Vector3 centre;
    }
}
