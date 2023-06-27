using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Field of 2d List of Cells
    private GameObject[,] _cells;
    private GameEngine _gameEngine;

    public List<GameObject> path;

    void Start()
    {
        _gameEngine = GlobalVariables.gameEngine;

        SubscribeToAllEvents();
        CreateCellGrid();
        LoadPath();
        UpdateAllCellPrefabsToMatchType();
        GlobalVariables.uiManager.SetupCamera(); // Maybe do this via event? Or call in game engine?
    }

    private void OnDestroy()
    {
        UnsubscribeToAllEvents();
    }

    private void SubscribeToAllEvents()
    {
        GlobalVariables.eventManager.cellEventManager.OnCellChange += OnCellChanged;
    }
    
    private void UnsubscribeToAllEvents()
    {
        GlobalVariables.eventManager.cellEventManager.OnCellChange -= OnCellChanged;
    }

    public Vector3 CalculateBoardCenter()
    {
        int totalCells = GlobalVariables.config.boardWidth * GlobalVariables.config.boardHeight;
        Vector3 sumPosition = Vector3.zero;

        for (int x = 0; x < GlobalVariables.config.boardWidth; x++)
        {
            for (int y = 0; y < GlobalVariables.config.boardHeight; y++)
            {
                // Get the position of the current cell
                Vector3 cellPosition = _cells[x, y].transform.position;
                sumPosition += cellPosition;
            }
        }

        Vector3 center = sumPosition / totalCells;
        
        return center;
    }
    
    /**
     * Some cells may need to change how they look based on their data
     * Maybe the cell should just call it itself in update?
     */
    private void UpdateAllCellPrefabsToMatchType()
    {
        // Call update render on each cell
        foreach (GameObject cellObject in _cells)
        {
            // Get the Cell component of the cellObject
            ICell cell = GetCell(cellObject);

            // Call the UpdateRender method on the cell
            cell.UpdateCellPrefabToMatchType();
        }
    }

    public List<ICell> GetAllCells()
    {
        // return a list of all cells
        List<ICell> allCells = new List<ICell>();
        
        foreach (GameObject cellObject in _cells)
        {
            allCells.Add(GetCell(cellObject));
        }

        return allCells;
    }

    private void LoadPath()
    {
        path = new List<GameObject>();

        // Get the path from the config
        List<Vector2Int> pathPositions = GlobalVariables.config.pathRoadMap;

        // For each position in the path, get the cell at that position and add it to the path
        foreach (Vector2Int pathPosition in pathPositions)
        {
            
            GameObject cellObject = _cells[pathPosition.x, pathPosition.y];
            
            GetCell(cellObject).type = CellType.Dirt;
            path.Add(cellObject);
        }
    }

    private void CreateCellGrid()
    {
        // Instantiate cells using _gameEngine's width and height
        _cells = new GameObject[GlobalVariables.config.boardWidth, GlobalVariables.config.boardHeight];

        // Create the "pf_Cell" prefab in every cell
        for (int x = 0; x < GlobalVariables.config.boardWidth; x++)
        {
            for (int y = 0; y < GlobalVariables.config.boardHeight; y++)
            {
                // Instantiate the prefab // Set the cellObject's parent to this transform
                GameObject cellObject = Instantiate(RandomUtils.GetRateRandomItem(GlobalVariables.config.grassCellPrefabs), transform, true);

                // Set the cellObject's position to the current x and y
                cellObject.transform.position = new Vector3(x, 0, y);
                
                // Randomly rotate the cellObject by 90 degrees
                cellObject.transform.Rotate(0, Random.Range(0, 4) * 90, 0);

                // Get the Cell component of the cellObject
                Cell cell = cellObject.GetComponent<Cell>();
                cell.x = x;
                cell.y = y;
                
                // Get a random number between 0 and 1
                float random = Random.Range(0f, 1f);
                
                // If the random number is less than the treeSpawnRate, set the cell type to a tree
                if (random < GlobalVariables.config.treeSpawnRate)
                {
                    cell.type = CellType.Tree;
                }

                // Set the cell in the _cells array
                _cells[x, y] = cellObject;
            }
        }
    }

    // Given a GameObject, get the cell script at the GameObject's position
    private ICell GetCell(GameObject cellObject)
    {
        // Get the Cell component of the gameObject
        ICell cell = cellObject.GetComponent<ICell>();

        // Return the cell
        return cell;
    }
    
    // Create a function to call when a cell is changed
    private void OnCellChanged(ICell oldCell, ICell newCell)
    {
        // Change our reference
        _cells[oldCell.x, oldCell.y] = newCell.GetGameObject();
        
        // if old cell was in path, remove it and put new cell, at the same location
        while (path.Contains(oldCell.GetGameObject())) // While because a cell can appear in the path multiple times (like a loop)
        {
            // Get the index of the old cell in the path
            int index = path.IndexOf(oldCell.GetGameObject());
            
            // Remove the old cell from the path
            path.RemoveAt(index);
            
            // Insert the new cell at the same index
            path.Insert(index, newCell.GetGameObject());
        }
    }

    public void PlaceTowerOn(ICell cell, ITower tower)
    {
        if (cell.IsBuildable())
        {
            // Get the cell gameObject
            GameObject cellObject = cell.GetGameObject();
            
            //place the ITower on the cell
            GameObject towerObject = tower.GetGameObject();
            towerObject.transform.position = GraphicsUtils.GetTopOf3d(cellObject);
            
            towerObject.GetComponent<Animator>().SetBool("isPlacing", false);
        
            // Set the tower's parent to the cell, so that it shows under cell in the object hierarchy
            towerObject.transform.SetParent(cellObject.transform);

            // Add tower to occupying game objects
            cell.occupyingGameObjects.Add(towerObject);
        }
    }
}
