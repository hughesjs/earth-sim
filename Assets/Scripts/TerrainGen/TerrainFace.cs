using System;
using UnityEngine;

namespace TerrainGen
{
    public class TerrainFace
    {
        private Mesh _mesh;
        private int _resolution;
        private Vector3 _localUp;
        private Vector3 _axisA;
        private Vector3 _axisB;

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
            Vector3[] vertices = new Vector3[_resolution * _resolution];
            int totalTris = 6 * (_resolution - 1) * (_resolution - 1);
            int[] tris = new int[totalTris];

            for (int y = 0, i = 0, t = 0; y < _resolution; y++)
            {
                for (int x = 0; x < _resolution; x++, i++)
                {
                    Vector2 ratio = new Vector2(x, y) / (_resolution - 1);
                    // Start at origin, need values between (-1,1,-1) and (1,1,1)
                    // So we need to go up one, subtract a half to get on the line
                    // and then add twice the progress along the axis line
                    // (it's twice because there's a distance of 2 between -1 and 1)
                    Vector3 pointOnUnitCube = _localUp +
                                              ((ratio.x - .5f) * 2 * _axisA) +
                                              ((ratio.y - .5f) * 2 * _axisB);

                    vertices[i] = pointOnUnitCube;

                    if (x < _resolution - 1 && y < _resolution - 1)
                    {
                        tris[t++] = i;
                        tris[t++] = i + _resolution + 1;
                        tris[t++] = i + _resolution;
                        tris[t++] = i;
                        tris[t++] = i + 1;
                        tris[t++] = i + _resolution + 1;
                    }
                }
            }

            _mesh.Clear();
            _mesh.vertices = vertices;
            _mesh.triangles = tris;
            _mesh.RecalculateNormals();
        }
    }
}
