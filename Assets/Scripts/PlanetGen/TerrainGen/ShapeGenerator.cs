using System.Linq;
using PlanetGen.Settings;
using UnityEngine;
using Vendor.Noise;

namespace PlanetGen.TerrainGen
{
    public class ShapeGenerator
    {
        private readonly ShapeSettings _settings;

        public ShapeGenerator(ShapeSettings settings)
        {
            _settings = settings;
            foreach (NoiseLayer layer in _settings.noiseLayers)
            {
                layer.noiseFilter = new(layer.noiseSetting);
            }
        }

        public Vector3 CalculatePointOnPlanet(Vector3 unitSpherePoint)
        {
            float firstLayerValue = 0.0f;
            float elevation = 0.0f;
            
            if (_settings.noiseLayers.Length > 0)
            {
                NoiseLayer firstLayer = _settings.noiseLayers.First();
                firstLayerValue = firstLayer.noiseFilter.EvaluateAtPoint(unitSpherePoint);

                if (firstLayer.enabled)
                {
                    elevation = firstLayerValue;
                }
            }

            elevation += _settings.noiseLayers
                                  .Skip(1)
                                  .Where(l => l.enabled)
                                  .Select(layer => new
                                                   {
                                                       layer,
                                                       // Should the mask just be binary??
                                                       mask = layer.useFirstLayerAsMask ? firstLayerValue : 1.0f
                                                   })
                                  .Select(t => t.layer.noiseFilter.EvaluateAtPoint(unitSpherePoint) * t.mask)
                                  .Sum();

            return unitSpherePoint * _settings.radius * (1 + elevation);
        }
    }
}
