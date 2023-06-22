using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public int boardWidth;
    public int boardHeight;

    [SerializedDictionary("prefab", "spawnRate")]
    public SerializedDictionary<GameObject, float> grassCellPrefabs;
    
    [SerializedDictionary("prefab", "spawnRate")]
    public SerializedDictionary<GameObject, float> treeCellPrefabs;
    
    [SerializedDictionary("prefab", "spawnRate")]
    public SerializedDictionary<GameObject, float> dirtCellPrefabs;
    
    public List<Wave> waves;

    public GameObject towerPrefab;  
    
    public GameObject towerCardPrefab;

    public float treeSpawnRate;
    
    public bool showDebugInfo;

    public DeckPreset DeckPreset;
}
