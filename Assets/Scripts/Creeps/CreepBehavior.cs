using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepBehavior : MonoBehaviour, ICreep
{

    public Dictionary<StatType, float> stats { get; set; }
    public int currentPathIndex { get; set; }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
