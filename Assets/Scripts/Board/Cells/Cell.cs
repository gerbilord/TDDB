using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, ICell
{
    // Field: List of game objects on cell
    public List<GameObject> occupyingGameObjects { get; set; }

    public int x { get; set; }
    public int y { get; set; }
    public virtual bool IsBuildable() { return true; }

    public CellType type { get; set; }

    // When created set the list of game objects to empty
    private void Awake()
    {
        occupyingGameObjects = new List<GameObject>();
    }
    public virtual void UpdateCellPrefabToMatchType()
    {
        // Create lambda function
        Action<GameObject> changeThisTo = prefab =>
        {
            GameObject newCell = Instantiate(prefab);
            ICell cellScript = newCell.GetComponent<ICell>();
            cellScript.SetDataFromCell(this);
            
            GlobalVariables.eventManager.cellEventManager.CellChanged(this, cellScript);
            Destroy(this.gameObject);
        };

        Type classType = this.GetType();

        if ( type == CellType.Dirt && classType != typeof(DirtCell))
        {
            changeThisTo(Utils.GetRateRandomItem(GlobalVariables.config.dirtCellPrefabs));
        } else if (type == CellType.Tree && classType != typeof(TreeCell))
        {
            changeThisTo(Utils.GetRateRandomItem(GlobalVariables.config.treeCellPrefabs));
        }
        else if (classType != typeof(GrassCell))
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
        type = cell.type;

        occupyingGameObjects = cell.occupyingGameObjects;
        
        // Get passed in cell's transform and set this transform to same position and rotation
        transform.position = cell.GetTransform().position;
        transform.rotation = cell.GetTransform().rotation;
    }

    private void OnMouseDown()
    {
        GlobalVariables.eventManager.cellEventManager.CellClicked(this);
    }
}

// Create enum CellType
public enum CellType
{
    Grass,
    Tree,
    Dirt
}