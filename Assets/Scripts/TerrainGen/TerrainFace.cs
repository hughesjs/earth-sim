using System.Collections.Generic;
using UnityEngine;

namespace TerrainGen
{
    public class TerrainFace
    {
        public readonly Mesh _mesh;
        private readonly int _resolution;
        private readonly Vector3 _localUp;
        private readonly Vector3 _axisA;
        private readonly Vector3 _axisB;

        public TerrainFace(Mesh mesh, int resolution, Vector3 localUp)
        {
            _mesh = mesh;
            _resolution = resolution;
            _localUp = localUp;

            _axisA = new(localUp.y, localUp.z, localUp.x);
            _axisB = Vector3.Cross(localUp, _axisA);
        }

        public void ConstructMesh()
        {
            List<Vector3> vertices = CreateVertices();
            List<int> triMap = CreateTriMap();

            _mesh.Clear();
            _mesh.vertices = vertices.ToArray();
            _mesh.triangles = triMap.ToArray();
            _mesh.RecalculateNormals();
        }

        private List<Vector3> CreateVertices()
        {
            List<Vector3> vertices = new(_resolution * _resolution);
            
            for (int y = 0; y < _resolution; y++)
            {
                for (int x = 0; x < _resolution; x++)
                {
                    Vector2 ratio = new Vector2(x, y) / (_resolution - 1);
                    // Start at origin, need values between (-1,1,-1) and (1,1,1)
                    // So we need to go up one and subtract a half to get on the line
                    // and then add twice the progress along the axis line
                    // (it's twice because there's a distance of 2 between -1 and 1)
                    Vector3 pointOnUnitCube = _localUp +
                                              ((ratio.x - .5f) * 2 * _axisA) +
                                              ((ratio.y - .5f) * 2 * _axisB);

                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                    vertices.Add(pointOnUnitSphere);
                }
            }
            return vertices;
        }

        private List<int> CreateTriMap()
        {
            int totalTris = 6 * (_resolution - 1) * (_resolution - 1);
            List<int> triMap = new(totalTris);
            for (int y = 0, i = 0; y < _resolution; y++) 
            {
                for (int x = 0; x < _resolution; x++, i++)
                {
                    if (x >= _resolution - 1 || y >= _resolution - 1) // Last row and column would be mapping off-mesh
                    {
                        continue;
                    }

                    triMap.Add(i);
                    triMap.Add(i + _resolution + 1);
                    triMap.Add(i + _resolution);
                    triMap.Add(i);
                    triMap.Add(i + 1);
                    triMap.Add(i + _resolution + 1);
                }
            }
            return triMap;
        }
    }
}
       
    

