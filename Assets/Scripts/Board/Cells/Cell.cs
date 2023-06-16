using System;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour, ICell
{
    // Field: List of game objects on cell
    public List<GameObject> occupyingGameObjects { get; set; }

    public int x { get; set; }
    public int y { get; set; }
    public bool isBuildable { get; set; }
    public bool isPath { get; set; }  

    public virtual void UpdateRender()
    {
        // Create lambda function
        Action<GameObject> changeThisTo = prefab =>
        {
            GameObject newCell = Instantiate(prefab);
            ICell cellScript = newCell.GetComponent<ICell>();
            cellScript.SetDataFromCell(this);
            
            GlobalVariables.eventManager.CellChanged(this, cellScript);
            Destroy(this.gameObject);
        };

        Type myType = this.GetType();

        if (isPath && myType != typeof(DirtCell))
        {
            changeThisTo(Utils.GetRateRandomItem(GlobalVariables.config.dirtCellPrefabs));
        } else if (!isBuildable && myType != typeof(TreeCell))
        {
            changeThisTo(Utils.GetRateRandomItem(GlobalVariables.config.treeCellPrefabs));
        }
        else if (myType != typeof(GrassCell))
        {
            changeThisTo(Utils.GetRateRandomItem(GlobalVariables.config.grassCellPrefabs));
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void SetDataFromCell(ICell cell)
    {
        x = cell.x;
        y = cell.y;
        isBuildable = cell.isBuildable;
        isPath = cell.isPath;
        
        occupyingGameObjects = cell.occupyingGameObjects;
        
        // Get passed in cell's transform and set this transform to same position and rotation
        transform.position = cell.GetTransform().position;
        transform.rotation = cell.GetTransform().rotation;
    }

    private void OnMouseDown()
    {
        GlobalVariables.eventManager.CellClicked(this);
    }
}
