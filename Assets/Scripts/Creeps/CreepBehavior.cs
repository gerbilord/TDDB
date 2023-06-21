using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepBehavior : MonoBehaviour, ICreep
{

    public float health { get; set; }
    public int currentPathIndex { get; set; }
    public float creepMoveSpeed { get; set; }
    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
