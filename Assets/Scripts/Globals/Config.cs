using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public bool showDebugInfo;

    public int boardWidth;
    public int boardHeight;
    
    public List<Vector2Int> pathRoadMap;

    [SerializedDictionary("prefab", "spawnRate")]
    public SerializedDictionary<GameObject, float> grassCellPrefabs;
    
    [SerializedDictionary("prefab", "spawnRate")]
    public SerializedDictionary<GameObject, float> treeCellPrefabs;
    
    [SerializedDictionary("prefab", "spawnRate")]
    public SerializedDictionary<GameObject, float> dirtCellPrefabs;
    
    public List<Wave> waves;

    public float treeSpawnRate;

    public DeckPreset DeckPreset;
    public int startingCardAmount;
}
