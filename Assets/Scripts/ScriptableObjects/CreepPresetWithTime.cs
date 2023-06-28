using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class CreepPresetWithTime
{
    public CreepPreset creepPreset;
    public float timeTillNextCreep;
    
    public CreepPresetWithTime(CreepPreset creepPreset, float timeTillNextCreep)
    {
        this.creepPreset = creepPreset;
        this.timeTillNextCreep = timeTillNextCreep;
    }
}
