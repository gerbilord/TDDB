using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "CreepPreset", menuName = "ScriptableObjects/CreepPreset", order = 1)]
public class CreepPreset : ScriptableObject
{
    public GameObject prefab;
    [SerializedDictionary("StatType", "value")]
    public SerializedDictionary<StatType, float> stats;
    public GameObject makeCreep()
    {
        GameObject newCreep = Instantiate(prefab);
        Dictionary<StatType, float> newStats = new Dictionary<StatType, float>(stats);
        newCreep.GetComponent<ICreep>().stats = newStats;
        return newCreep;
    }

}