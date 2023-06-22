using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerPreset", menuName = "ScriptableObjects/TowerPreset", order = 1)]
public class TowerPreset : ScriptableObject
{
    public GameObject prefab;
    public BulletPreset bulletPreset;
    [SerializedDictionary("StatType", "value")]
    public SerializedDictionary<StatType, float> stats;
    public GameObject makeTower()
    {
        GameObject newTower = Instantiate(prefab);
        Dictionary<StatType, float> newStats = new Dictionary<StatType, float>(stats);
        newTower.GetComponent<ITower>().stats = newStats;
        newTower.GetComponent<ITower>().bulletPreset = bulletPreset;
        newTower.GetComponent<ITower>().towerTargetBehavior = new LastCreepTargetBehavior();
        return newTower;
    }
    
}