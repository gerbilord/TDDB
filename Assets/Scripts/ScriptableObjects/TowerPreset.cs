using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerPreset", menuName = "ScriptableObjects/TowerPreset", order = 1)]
public class TowerPreset : ScriptableObject
{
    [SerializedDictionary("StatType", "value")]
    public SerializedDictionary<StatType, float> stats;
    
    public GameObject prefab;
    public TowerTargetingPreset towerTargetingPreset;
    public BulletPreset bulletPreset;

    public ITower makeTower()
    {
        GameObject newTowerObj = Instantiate(prefab);
        Dictionary<StatType, float> newStats = new Dictionary<StatType, float>(stats);
        
        ITower newTower = newTowerObj.GetComponent<ITower>(); 
        newTower.stats = newStats;
        newTower.bulletPreset = bulletPreset;
        newTower.towerTargetBehavior = towerTargetingPreset.MakeTowerTargetingBehavior();
        return newTower;
    }
    
}