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
            Vector3 configuredPoint = point * _noiseSettings.roughness + _noiseSettings.centre;
            float noiseVal = _noise.Evaluate(configuredPoint); // Range of -1 to 1
            float adjustedNoiseVal = TranslateToDesiredRange(noiseVal);
            return adjustedNoiseVal * _noiseSettings.strength;
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
