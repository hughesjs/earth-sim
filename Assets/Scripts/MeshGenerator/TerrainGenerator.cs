using UnityEngine;

// Ref: http://www.martin-ritter.com/2019/02/unity-simple-mesh-generation/

namespace MeshGenerator
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class TerrainGenerator : MonoBehaviour
    {
        public int xSize;
        public int zSize;

        public float stepTimeInSeconds;

        public MeshRenderer meshRenderer;
        public MeshFilter meshFilter;

        private Vector3[] _vertices;
        private int[] _tris;
        private Mesh _mesh;

        // Start is called before the first frame update
        void Start()
        {
            ConnectComponents();
            GenerateCube();
        }

        private void ConnectComponents()
        {
            Debug.Log("Connecting components");
            _mesh ??= new();
            meshRenderer ??= GetComponent<MeshRenderer>();
            meshFilter ??= GetComponent<MeshFilter>();

            meshFilter.sharedMesh = _mesh;
        }

        private void GenerateCube()
        {
            Debug.Log("Generating Cube");
            GenerateInitialVertices();
            GenerateTris();
            BuildMesh();
        }

        private void BuildMesh()
        {
            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.triangles = _tris;
            _mesh.RecalculateNormals();
        }

        private int xVertices => xSize + 1;
        private int yVertices => 0;
        private int zVertices => zSize + 1;
        private void GenerateInitialVertices()
        {
            Debug.Log("Creating Vertices");
            int totalVertices = xVertices * zVertices;
            _vertices = new Vector3[totalVertices];
    
            for (int z = 0, i = 0; z < zVertices; z++) //Note i = 0 declared here
            {
                for (int x = 0; x < zVertices; x++, i++) // i incremented here
                {
                    _vertices[i] = new(x, 0, z);

                    Debug.Log($"Produced new vertex ({x};0;{z})");
                }
            }
        }

        private void GenerateTris()
        {
            Debug.Log("Creating Tris");
            _tris = new int[xSize * zSize * 6]; // 6 edges per quad (diagonal counts twice, once for each tri)
            for (int v = 0, t = 0; v < _vertices.Length; v++)
            {
                if (_vertices[v].x >= xSize || _vertices[v].z >= zSize)
                {
                    continue;
                }
                _tris[t++] = v;
                _tris[t++] = v + xSize + 1;
                _tris[t++] = v + 1;
                _tris[t++] = v + 1;
                _tris[t++] = v + xSize + 1;
                _tris[t++] = v + xSize + 2;
            }
        }
    
        private void OnDrawGizmos()
        {

            if (_vertices is not null)
            {
                foreach (Vector3 t in _vertices)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(t, 0.0666f);
                }
            }

            if (_tris is not null && _vertices is not null)
            {
                for (int i = 0; i < _tris.Length - 2; i += 3)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(_vertices[_tris[i]], _vertices[_tris[i + 1]]);
                    Gizmos.DrawLine(_vertices[_tris[i + 1]], _vertices[_tris[i + 2]]);
                    Gizmos.DrawLine(_vertices[_tris[i + 2]], _vertices[_tris[i]]);
                }
            }
        }   
    }
}