using System;
using PlanetGen.TerrainGen;
using UnityEngine;

namespace PlanetGen.Settings
{
    [CreateAssetMenu]
    public class ShapeSettings : ScriptableObject
    {
        public float radius;
        public NoiseLayer[] noiseLayers;
    }
    
    [Serializable]
    public class NoiseLayer
    {
        public string description;
        public bool enabled = true;
        public bool useFirstLayerAsMask = true;
        public NoiseSettings noiseSetting;
        public NoiseFilter noiseFilter;
    }
}
