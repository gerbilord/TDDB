using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet
{
    public Dictionary<StatType, float> stats { get; set; }
    public GameObject GetGameObject();
    public void Seek(ICreep target, ITower tower);
    

}