using System;
using PlanetGen.Settings;
using UnityEngine;
using Vendor.Noise;

namespace PlanetGen.TerrainGen
{
    public class NoiseFilter
    {
        private readonly NoiseSettings _noiseSettings;
        private readonly Noise _noise = new();

        private const float DesiredNoiseMin = 0;
        private const float DesiredNoiseMax = 1;

        private const float RawNoiseMax = 1;
        private const float RawNoiseMin = -1;

        public NoiseFilter(NoiseSettings settings)
        {
            _noiseSettings = settings;
        }

        public float EvaluateAtPoint(Vector3 point)
        {
            float amplitude = 1.0f;
            float noiseVal = 0.0f;
            float frequency = _noiseSettings.baseRoughness;
         
            for (int i = 0; i < _noiseSettings.numLayers; i++)
            {
                Vector3 configuredPoint = point * frequency + _noiseSettings.centre;
                float v = _noise.Evaluate(configuredPoint); // Range of -1 to 1
                noiseVal += TranslateToDesiredRange(v) * amplitude;

                frequency *= _noiseSettings.roughness;
                amplitude *= _noiseSettings.persistence;
            }

            noiseVal = Mathf.Max(_noiseSettings.minValue, noiseVal - _noiseSettings.minValue);
            return noiseVal * _noiseSettings.strength;
        }

        private static float TranslateToDesiredRange(float noiseVal)
        {
            // Examples will use desired range of 0 to 2
            const float translationAdjust = DesiredNoiseMin - RawNoiseMin; //  0 - (-1) = 1
            const float scaleAdjust = (DesiredNoiseMax - DesiredNoiseMin) / (RawNoiseMax - RawNoiseMin); // (1 - 0) / (1 - (-1))

            return scaleAdjust * (noiseVal + translationAdjust);
        }
    }
}
