using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICreep
{
    public Dictionary<StatType, float> stats { get; set; }
    int currentPathIndex { get; set; }
    public GameObject GetGameObject();

    public bool takeBulletHit(IBullet bullet);
    public void killCreep();
}
