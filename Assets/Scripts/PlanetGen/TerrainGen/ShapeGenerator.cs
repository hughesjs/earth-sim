using PlanetGen.Settings;
using UnityEngine;

namespace PlanetGen.TerrainGen
{
    public class ShapeGenerator
    {
        private readonly ShapeSettings _settings;
        
        public ShapeGenerator(ShapeSettings settings) => _settings = settings;

        public Vector3 CalculatePointOnPlanet(Vector3 unitSpherePoint)
        {
            return unitSpherePoint * _settings.radius;
        }
    }
}
