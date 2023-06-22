using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard
{
    public GameObject GetGameObject();
    public ITower tower { get; set; }
    public TowerPreset towerPreset { get; set; }

    public void Play(ICell cell);

}
