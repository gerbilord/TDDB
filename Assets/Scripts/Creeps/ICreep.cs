using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICreep
{
    float health { get; set; }
    int currentPathIndex { get; set; }
    float creepMoveSpeed { get; set; }

    public GameObject GetGameObject();
}
