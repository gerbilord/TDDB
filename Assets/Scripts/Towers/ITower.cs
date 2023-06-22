using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITower
{
    public Dictionary<StatType, float> stats { get; set; }
    public GameObject GetGameObject();
    public ITowerTargetBehavior towerTargetBehavior { get; set; }

    public BulletPreset bulletPreset { get; set; }

}