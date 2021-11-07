using TerrainGen;
using UnityEngine;

namespace Planet
{
    public class Planet : MonoBehaviour
    {
        [Range(2, 256)]
        public int resolution = 10;
        
        private const int Faces = 6; // This will break stuff if you change it... cubes have 6 faces
        
        [SerializeField, HideInInspector] 
        private MeshFilter[] filters;
        private TerrainFace[] _terrainFaces;

        private readonly Vector3[] _faceDirections = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        private void OnValidate()
        {
            Initialize();
            GenerateMesh();
        }
        
        private void Initialize()
        {
            _terrainFaces = new TerrainFace[Faces]; // Regen on update

            if (filters is null || filters.Length <= 0)
            {
                filters = new MeshFilter[Faces];
            }
            
            for (int i = 0; i < Faces; i++)
            {
                if (filters[i] is null) // Don't regen on update
                {
                    GameObject mesh = new("mesh");
                    mesh.transform.parent = transform;
                    mesh.AddComponent<MeshRenderer>().sharedMaterial = new(Shader.Find("Standard"));
                    MeshFilter meshFilter = mesh.AddComponent<MeshFilter>();
                    meshFilter.sharedMesh = new();
                    filters[i] = meshFilter;
                }
                
                _terrainFaces[i] = new(filters[i].sharedMesh, resolution, _faceDirections[i]);
            }
        }

        void GenerateMesh()
        {
            foreach (TerrainFace face in _terrainFaces)
            {
                face.ConstructMesh();
            }
        }
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
