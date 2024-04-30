using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour {
    Vector3[] worldPoints;
    // Noise parameters
    [SerializeField] int mapWidth = 20;
    [SerializeField] int mapHeight = 20;
    [SerializeField] float noiseScale;
    [SerializeField] int octaves;
    [Range(0f, 1f)]
    [SerializeField] float persistance;
    [SerializeField] float lacunarity;
    [SerializeField] int seed;
    [SerializeField] Vector2 offset;
    [SerializeField] TerrainType[] tiles;
    [SerializeField] AdditionalObjects[] objectsOnTerrain;

    bool isTileEmpty;




    private void Start() {
        GenerateMap();
    }

    public void GenerateMap() {
        for (var i = transform.childCount - 1; i >= 0; i--) {
            Object.DestroyImmediate(transform.GetChild(i).gameObject);
        }

        worldPoints = new Vector3[(mapWidth + 1) * (mapHeight + 1)];
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        for (int i = 0, y = 0; y < mapHeight; y++) {
            for (int x = 0; x < mapWidth; x++) {
                isTileEmpty = true;
                float currentHeight = noiseMap[x, y];
                // Make sure noise value is between 0 and 1
                currentHeight = NoiseNormalized(currentHeight);
                worldPoints[i] = new Vector3(x, y, 0);
                // Generating ground tiles
                foreach (TerrainType t in tiles) {
                    if (currentHeight >= t.noiseHeightMin && currentHeight < t.noiseHeightMax) {
                        GameObject child = Instantiate(t.prefab, worldPoints[i], Quaternion.identity, this.transform);
                        break;
                    };
                }

                // Generating additional assets
                foreach (AdditionalObjects newObject in objectsOnTerrain) {
                    if (isTileEmpty) {
                        GenerateObject(currentHeight, newObject.prefabArray, newObject.spawnProbability, worldPoints[i]);
                    }
                }
                i++;
            }
        }
    }
    private void GenerateObject(float currentNoiseHeight, GameObject[] assets, float objectSpawnProbability, Vector3 position) {
        if (currentNoiseHeight <= objectSpawnProbability && isTileEmpty) {
            GameObject gameobject = assets[Random.Range(0, assets.Length)];
            Instantiate(gameobject, position, Quaternion.identity, this.transform);
            isTileEmpty = false;
            
        }
    }

    private float NoiseNormalized(float noiseHeight) {
        if (noiseHeight <= 0) {
            noiseHeight = 0.0001f;
        } else if (noiseHeight >= 1) {
            noiseHeight = 0.99f;
        }
        return noiseHeight;
    }
    
    // Make sure there is no posibility for wrong input
    private void OnValidate() {
        if (mapWidth < 1) {
            mapWidth = 1;
        }
        if (mapHeight < 1) {
            mapHeight = 1;
        }
        if (lacunarity < 1) {
            lacunarity = 1;
        }
        if (octaves < 0) {
            octaves = 0;
        }
    }

    [System.Serializable]
    public struct TerrainType {
        public string name;
        public float noiseHeightMin;
        public float noiseHeightMax;
        public GameObject prefab;
    }
    [System.Serializable]
    public struct AdditionalObjects {
        public string name;
        public GameObject[] prefabArray;
        [Range(0f, 1f)]
        [SerializeField] public float spawnProbability;
    }

}
