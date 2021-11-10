using UnityEngine;
using Vendor.Noise;

namespace PlanetGen.TerrainGen
{
    public class NoiseFilter
    {
        private readonly Noise _noise = new();

        private const float Min = 0;
        private const float Max = 1;

        private const float RawMax = 1;
        private const float RawMin = -1;

        public float EvaluateAtPoint(Vector3 point)
        {
            float noiseVal = _noise.Evaluate(point); // Range of -1 to 1
            return TranslateToDesiredRange(noiseVal);
        }

        private float TranslateToDesiredRange(float noiseVal)
        {
            // Examples will use desired range of 0 to 2
            float translationAdjust = Min - RawMin; //  0 - (-1) = 1
            float scaleAdjust = (Max - Min) / (RawMax - RawMin); // (1 - 0) / (1 - (-1))

            return scaleAdjust * (noiseVal + translationAdjust);
        }
    }
}
