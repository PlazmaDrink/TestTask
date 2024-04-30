using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainGenerator))]
[CanEditMultipleObjects]
public class TerrainGeneratorEditor : Editor {

    //properties ------------
    SerializedProperty mapWidth;
    SerializedProperty mapHeight;
    SerializedProperty noiseScale;
    SerializedProperty octaves;
    SerializedProperty persistance;
    SerializedProperty lacunarity;
    SerializedProperty seed;
    SerializedProperty offset;
    SerializedProperty regions;

    //fold outs --------------
    bool noiseParameters = false;

    public override void OnInspectorGUI() {
        serializedObject.Update();
        TerrainGenerator mapGen = (TerrainGenerator)target;

        noiseParameters = EditorGUILayout.Foldout(noiseParameters, "Noise Parameters");
        if (noiseParameters) {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Define noise parameters", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(mapWidth);
            EditorGUILayout.PropertyField(mapHeight);
            EditorGUILayout.PropertyField(noiseScale);
            EditorGUILayout.PropertyField(octaves);
            EditorGUILayout.PropertyField(persistance);
            EditorGUILayout.PropertyField(lacunarity);
            EditorGUILayout.PropertyField(seed);
            EditorGUILayout.PropertyField(offset);
            EditorGUILayout.PropertyField(regions);

        }

        if (DrawDefaultInspector()) {
                mapGen.GenerateMap();   
        };

        if (GUILayout.Button("Generate")) {
            mapGen.GenerateMap();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable() {
        mapWidth = serializedObject.FindProperty("mapWidth");
        mapHeight = serializedObject.FindProperty("mapHeight");
        noiseScale = serializedObject.FindProperty("noiseScale");
        octaves = serializedObject.FindProperty("octaves");
        persistance = serializedObject.FindProperty("persistance");
        lacunarity = serializedObject.FindProperty("lacunarity");
        seed = serializedObject.FindProperty("seed");
        offset = serializedObject.FindProperty("offset");
        regions = serializedObject.FindProperty("regions");


    }

}
