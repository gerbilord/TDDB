using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, ICell
{
    public IGameEngine gameEngine { get; set; }

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

    public void Setup(IGameEngine gameEngine)
    {
        this.gameEngine = gameEngine;
    }

    public virtual void UpdateCellPrefabToMatchType()
    {
        // Create lambda function
        Action<GameObject> changeThisTo = prefab =>
        {
            // Create a new cell, make sure its still under the same parent (board)
            GameObject newCell = Instantiate(prefab, this.transform.parent, true);
            ICell cellScript = newCell.GetComponent<ICell>();
            cellScript.SetDataFromCell(this);
            
            GlobalVariables.eventManager.cellEventManager.CellChanged(this, cellScript);
            Destroy(this.gameObject);
        };

        Type classType = this.GetType();

        if ( type == CellType.Dirt && classType != typeof(DirtCell))
        {
            changeThisTo(RandomUtils.GetRateRandomItem(gameEngine.config.dirtCellPrefabs));
        } else if (type == CellType.Tree && classType != typeof(TreeCell))
        {
            changeThisTo(RandomUtils.GetRateRandomItem(gameEngine.config.treeCellPrefabs));
        }
        else if (classType != typeof(GrassCell))
        {
            changeThisTo(RandomUtils.GetRateRandomItem(gameEngine.config.grassCellPrefabs));
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
        
        gameEngine = cell.gameEngine;
    }

    private void OnMouseDown()
    {
        GlobalVariables.eventManager.cellEventManager.CellClicked(this);
    }
    
    private void OnMouseEnter()
    {
        GlobalVariables.eventManager.cellEventManager.CellMouseEntered(this);
    }
    
    private void OnMouseExit()
    {
        GlobalVariables.eventManager.cellEventManager.CellMouseExited(this);
    }
}

// Create enum CellType
public enum CellType
{
    Grass,
    Tree,
    Dirt
}