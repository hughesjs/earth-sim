using System;
using System.Collections.Generic;
using PlanetGen;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object; // Because who thought having a class named object was okay?
namespace Editor
{
    [CustomEditor(typeof(Planet))]
    public class PlanetEditor : UnityEditor.Editor
    {
        private const string ShapeFoldoutKey = "Planet.ShapeSettings/FoldOut";
        private const string ColourFoldoutKey = "Planet.ColourSettings/FoldOut";
        
        private Planet _planet;
        private Dictionary<ScriptableObject, EditorMap> _editorConfig;

        public override void OnInspectorGUI()
        {
            using EditorGUI.ChangeCheckScope changeCanary = new();
            base.OnInspectorGUI();
            if (changeCanary.changed)
            {
                _planet.GenerateNewPlanet();
            }

            if (GUILayout.Button("Randomise Planet"))
            {
                _planet.colourSettings.baseColour = UnityEngine.Random.ColorHSV();
                _planet.shapeSettings.radius = UnityEngine.Random.value + 0.1f * 2f;
                _planet.GenerateNewPlanet();
            }
            
            if (GUILayout.Button("Repair Planet"))
            {
                _planet.GenerateNewPlanet();
            }

            foreach (KeyValuePair<ScriptableObject, EditorMap> kvp in _editorConfig)
            {
                DrawSettingsEditor(kvp.Key, kvp.Value);
            }
        }

        private void DrawSettingsEditor(UnityObject settings, EditorMap editorMap)
        {
            if (settings == null) return;
            using EditorGUI.ChangeCheckScope changeCanary = new();
            var foldout = EditorPrefs.GetBool(editorMap.FoldOutKey, false);
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            EditorPrefs.SetBool(editorMap.FoldOutKey, foldout);
            if (foldout)
            {
                CreateCachedEditor(settings, null, ref editorMap.CachedEditor);
                
                editorMap.CachedEditor.OnInspectorGUI();
                if (changeCanary.changed)
                {
                    editorMap.CallBack();
                }
            }
        }

        private void OnEnable()
        {
            _planet = (Planet)target;
            _editorConfig = new()
                            {
                                {
                                    _planet.shapeSettings, new()
                                                           {
                                                               FoldOutKey = ShapeFoldoutKey,
                                                               CallBack = _planet.OnShapeSettingsUpdated,
                                                               CachedEditor = null
                                                           }
                                },
                                {
                                    _planet.colourSettings, new()
                                                            {
                                                                FoldOutKey = ColourFoldoutKey,
                                                                CallBack = _planet.OnColourSettingsUpdated,
                                                                CachedEditor = null
                                                            }
                                }
                            };
        }


        private class EditorMap
        {
            public string FoldOutKey;
            public Action CallBack;
            public UnityEditor.Editor CachedEditor;
        }
    }
}
