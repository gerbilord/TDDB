using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICell  
{
    public List<GameObject> occupyingGameObjects { get; set; }
    public int x { get; set; }
    public int y { get; set; }
    
    public bool isBuildable { get; set; }
    
    public bool isPath { get; set; }

    public void UpdateRender();
    
    public Transform GetTransform();
    
    public GameObject GetGameObject();
    
    public void SetDataFromCell(ICell cell);
    
}
