using PlanetGen.Settings;
using PlanetGen.TerrainGen;
using UnityEngine;

namespace PlanetGen
{
    public class Planet : MonoBehaviour
    {
        [Range(2, 256)]
        public int resolution = 10;
        public bool autoUpdate = true;
        public ColourSettings colourSettings;
        public ShapeSettings shapeSettings;
      
        [SerializeField, HideInInspector] 
        private MeshFilter[] filters;
        private TerrainFace[] _terrainFaces;
        private readonly Vector3[] _faceDirections = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        private ShapeGenerator _shapeGenerator;
        
        private const int Faces = 6; // This will break stuff if you change it... cubes have 6 faces

        public void GenerateNewPlanet()
        {
            Initialize();
            GenerateMesh();
            GenerateColours();
        }

        public void OnColourSettingsUpdated()
        {
            if (!autoUpdate) return;
            Initialize();
            GenerateColours();
        }
        
        public void OnShapeSettingsUpdated()
        {
            if (!autoUpdate) return;
            Initialize();
            GenerateMesh();
        }

        
        private void Initialize()
        {
            Debug.Log("Initialising planet mesh");
            _terrainFaces = new TerrainFace[Faces];
            filters = new MeshFilter[Faces];
            _shapeGenerator = new(shapeSettings);
            
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
            for (int i = 0; i < Faces; i++)
            {
                _terrainFaces[i] = new(_shapeGenerator, filters[i].sharedMesh, resolution, _faceDirections[i]);
            }
            foreach (TerrainFace face in _terrainFaces)
            {
                face.ConstructMesh();
            }
        }

        private void GenerateColours()
        {
            foreach (MeshFilter filter in filters)
            {
                filter.GetComponent<MeshRenderer>().sharedMaterial.color = colourSettings.baseColour;
            }
        }
    }
}
