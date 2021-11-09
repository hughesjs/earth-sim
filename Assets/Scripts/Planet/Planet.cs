using System;
using System.Linq;
using TerrainGen;
using UnityEngine;

namespace Planet
{
    public class Planet : MonoBehaviour
    {
        private readonly System.Random prng = new();
        private int count;

        [Range(2, 256)]
        public int resolution = 10;
        
        private const int Faces = 6; // This will break stuff if you change it... cubes have 6 faces
        
        [SerializeField, HideInInspector] 
        private MeshFilter[] filters;

        private TerrainFace[] _terrainFaces;

        private readonly Vector3[] _faceDirections = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        private void OnValidate()
        {
            if (filters is null || filters.Any(f => f == null) || _terrainFaces is null || _terrainFaces.Any(t => t is null) )
            {
                Initialize();
            }
            UpdateMesh();
            GenerateMesh();
        }

        private void UpdateMesh()
        {
            Debug.Log("Updating planet mesh");
            for (int i = 0; i < Faces; i++)
            {
                _terrainFaces[i] = new(filters[i].sharedMesh, resolution, _faceDirections[i]);
            }
        }
        
        private void Initialize()
        {
            Debug.Log("Initialising planet mesh");
            _terrainFaces = new TerrainFace[Faces];
            filters = new MeshFilter[Faces];

            for (int i = 0; i < Faces; i++)
            {
                string gObjName = $"Terrain Face {i}";
                GameObject terrainFaceObj = GameObject.Find(gObjName);
                if (terrainFaceObj == null) // Overloaded equality wanted
                {
                    terrainFaceObj = new(gObjName);
                    terrainFaceObj.transform.parent = transform;
                    terrainFaceObj.AddComponent<MeshRenderer>().sharedMaterial = new(Shader.Find("Standard"));
                }
                
                MeshFilter meshFilter = terrainFaceObj.GetComponent<MeshFilter>();
                if (meshFilter == null)
                {
                    meshFilter = terrainFaceObj.AddComponent<MeshFilter>();
                    meshFilter.sharedMesh = new();
                }
                
                filters[i] = meshFilter;
            }
        }

        private void GenerateMesh()
        {
            foreach (TerrainFace face in _terrainFaces)
            {
                face.ConstructMesh();
            }
        }
    }
}
