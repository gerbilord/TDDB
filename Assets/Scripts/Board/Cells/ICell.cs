using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICell
{
    public List<GameObject> occupyingGameObjects { get; set; }
    public int x { get; set; }
    public int y { get; set; }

    public CellType type { get; set; }

    public bool IsBuildable();

    public void UpdateCellPrefabToMatchType();
    
    public Transform GetTransform();
    
    public GameObject GetGameObject();
    
    public void SetDataFromCell(ICell cell);
    
}
