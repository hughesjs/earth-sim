using PlanetGen.Settings;
using UnityEngine;
using Vendor.Noise;

namespace PlanetGen.TerrainGen
{
    public class ShapeGenerator
    {
        private readonly ShapeSettings _settings;
        private readonly NoiseFilter _noiseFilter;

        public ShapeGenerator(ShapeSettings settings)
        {
            _settings = settings;
            _noiseFilter = new();
        }

        public Vector3 CalculatePointOnPlanet(Vector3 unitSpherePoint)
        {
            float elevation = _noiseFilter.EvaluateAtPoint(unitSpherePoint);
            return unitSpherePoint * _settings.radius * (1 + elevation);
        }
    }
}
