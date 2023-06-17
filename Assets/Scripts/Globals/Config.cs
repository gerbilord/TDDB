using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    // Get a reference to a prefab
    public List<GameObjectSpawnRate> grassCellPrefabs;
    public List<GameObjectSpawnRate> treeCellPrefabs;
    public List<GameObjectSpawnRate> dirtCellPrefabs;
    
    public GameObject towerPrefab;

    public float treeSpawnRate;
}
