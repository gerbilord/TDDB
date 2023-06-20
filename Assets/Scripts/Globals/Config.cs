using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    // Get a reference to a prefab
    public List<GameObjectSpawnRate> grassCellPrefabs;
    public List<GameObjectSpawnRate> treeCellPrefabs;
    public List<GameObjectSpawnRate> dirtCellPrefabs;
    
    public List<Wave> waves;
    
    public GameObject towerPrefab;
    
    public GameObject towerCardPrefab;

    public float treeSpawnRate;
}
