using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "CreepPreset", menuName = "ScriptableObjects/CreepPreset", order = 1)]
public class CreepPreset : ScriptableObject
{
    [SerializedDictionary("StatType", "value")]
    public SerializedDictionary<StatType, float> stats;
    
    public GameObject prefab;

    public ICreep makeCreep()
    {
        GameObject newCreep = Instantiate(prefab);
        Dictionary<StatType, float> newStats = new Dictionary<StatType, float>(stats);
        newCreep.GetComponent<ICreep>().stats = newStats;
        return newCreep.GetComponent<ICreep>();
    }

}