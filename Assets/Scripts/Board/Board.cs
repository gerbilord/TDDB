using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IHasIGameEngine
{
    public IGameEngine gameEngine { get; set; }

    // Field of 2d List of Cells
    private GameObject[,] _mainBoardCells;
    
    private GameObject[,] _corralCells;
    private GameObject[,] _immediateSendCells;

    public List<GameObject> path;

    public void Setup(IGameEngine gameEngine)
    {
        this.gameEngine = gameEngine;
        SubscribeToAllEvents();
        CreateMainBoardGrid();
        CreateCorralGrid();
        LoadPath();
        UpdateAllCellPrefabsToMatchType();
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
        int totalCells = gameEngine.config.boardWidth * gameEngine.config.boardHeight;
        Vector3 sumPosition = Vector3.zero;

        for (int x = 0; x < gameEngine.config.boardWidth; x++)
        {
            for (int y = 0; y < gameEngine.config.boardHeight; y++)
            {
                // Get the position of the current cell
                Vector3 cellPosition = _mainBoardCells[x, y].transform.position;
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
        foreach (GameObject cellObject in _mainBoardCells)
        {
            // Get the Cell component of the cellObject
            ICell cell = GetCell(cellObject);

            // Call the UpdateRender method on the cell
            cell.UpdateCellPrefabToMatchType();
        }
    }

    public List<ICell> GetAllCorralCells()
    {
        List<ICell> allCells = new List<ICell>();
        
        foreach (GameObject cellObject in _corralCells)
        {
            allCells.Add(GetCell(cellObject));
        }
        
        return allCells;
    }
    
    public List<ICell> GetAllImmediateSendCells()
    {
        List<ICell> allCells = new List<ICell>();
        
        foreach (GameObject cellObject in _immediateSendCells)
        {
            allCells.Add(GetCell(cellObject));
        }
        
        return allCells;
    }
    
    public List<ICell> GetAllMainBoardCells()
    {
        // return a list of all cells
        List<ICell> allCells = new List<ICell>();
        
        foreach (GameObject cellObject in _mainBoardCells)
        {
            allCells.Add(GetCell(cellObject));
        }

        return allCells;
    }

    public List<ICell> GetAllCells()
    {
        // return a list of all cells
        List<ICell> allCells = new List<ICell>();
        
        allCells.AddRange(GetAllMainBoardCells());
        allCells.AddRange(GetAllCorralCells());
        allCells.AddRange(GetAllImmediateSendCells());

        return allCells;
    }

    public bool IsCellOurCell(ICell cell)
    {
        return IsCellInCorral(cell) || IsCellInImmediateSend(cell) || IsCellInMainBoard(cell);
    }
    
    public bool IsCellInCorral(ICell cell)
    {
        // If the cell's x and y are out of bounds, its definitely not in the corral
        if(!CellCanBeInBoundsOfArray(cell, _corralCells))
        {
            return false;
        }
        
        // Check if the cell is in the corral, this is an efficient check, instead of looping over every item in the array.
        return _corralCells[cell.x, cell.y] == cell.GetGameObject();
    }
    
    public bool IsCellInImmediateSend(ICell cell)
    {
        // If the cell's x and y are out of bounds, its definitely not in the corral
        if(!CellCanBeInBoundsOfArray(cell, _immediateSendCells))
        {
            return false;
        }
        
        // Check if the cell is in the corral, this is an efficient check, instead of looping over every item in the array.
        return _immediateSendCells[cell.x, cell.y] == cell.GetGameObject();
    }
    
    public bool IsCellInMainBoard(ICell cell)
    {
        // If the cell's x and y are out of bounds, its definitely not in the corral
        if(!CellCanBeInBoundsOfArray(cell, _mainBoardCells))
        {
            return false;
        }
        
        // Check if the cell is in the corral, this is an efficient check, instead of looping over every item in the array.
        return _mainBoardCells[cell.x, cell.y] == cell.GetGameObject();
    }

    private bool CellCanBeInBoundsOfArray(ICell cell, GameObject[,] arrayToCheck)
    {
        // make sure cell.x and cell.y don't error
        if (cell.x < 0 || cell.y < 0)
        {
            return false;
        }
        
        if (cell.x >= arrayToCheck.GetLength(0) || cell.y >= arrayToCheck.GetLength(1))
        {
            return false;
        }

        return true;
    }

    private void LoadPath()
    {
        path = new List<GameObject>();

        // Get the path from the config
        List<Vector2Int> pathPositions = gameEngine.config.pathRoadMap;

        // For each position in the path, get the cell at that position and add it to the path
        foreach (Vector2Int pathPosition in pathPositions)
        {
            
            GameObject cellObject = _mainBoardCells[pathPosition.x, pathPosition.y];
            
            GetCell(cellObject).type = CellType.Dirt;
            path.Add(cellObject);
        }
    }

    private void CreateMainBoardGrid()
    {
        // Instantiate cells using _gameEngine's width and height
        _mainBoardCells = new GameObject[gameEngine.config.boardWidth, gameEngine.config.boardHeight];

        // Create the "pf_Cell" prefab in every cell
        for (int x = 0; x < gameEngine.config.boardWidth; x++)
        {
            for (int y = 0; y < gameEngine.config.boardHeight; y++)
            {
                // Instantiate the prefab // Set the cellObject's parent to this transform
                GameObject cellObject = Instantiate(RandomUtils.GetRateRandomItem(gameEngine.config.grassCellPrefabs), transform, true);

                // Set the cellObject's position to the current x and y
                cellObject.transform.position = new Vector3(x, 0, y);
                
                // Randomly rotate the cellObject by 90 degrees
                cellObject.transform.Rotate(0, Random.Range(0, 4) * 90, 0);

                // Get the Cell component of the cellObject
                Cell cell = cellObject.GetComponent<Cell>();
                cell.Setup(gameEngine);
                cell.x = x;
                cell.y = y;
                
                // Get a random number between 0 and 1
                float random = Random.Range(0f, 1f);
                
                // If the random number is less than the treeSpawnRate, set the cell type to a tree
                if (random < gameEngine.config.treeSpawnRate)
                {
                    cell.type = CellType.Tree;
                }

                // Set the cell in the _cells array
                _mainBoardCells[x, y] = cellObject;
            }
        }
    }
    
    private void CreateCorralGrid()
    {
        // Instantiate cells using _gameEngine's width and height
        _corralCells = new GameObject[gameEngine.config.corralWidth, gameEngine.config.corralHeight];
        _immediateSendCells = new GameObject[gameEngine.config.corralWidth, gameEngine.config.corralHeight];
        
        CreateUIOnlyFenceAroundCorral();
        
        // Create the "pf_Cell" prefab in every corral cell, to the right of the board, separated by a gap
        for (int x = 0; x < gameEngine.config.corralWidth; x++)
        {
            for (int y = 0; y < gameEngine.config.corralHeight; y++)
            {
                // Instantiate the prefab // Set the cellObject's parent to this transform
                GameObject cellObject = Instantiate(RandomUtils.GetRateRandomItem(gameEngine.config.grassCellPrefabs), transform, true);

                // Set the cellObject's position to the current x and y, plus the width of the board, plus the gap
                cellObject.transform.position = new Vector3(x + gameEngine.config.boardWidth + gameEngine.config.corralGap, 0, y);

                // Randomly rotate the cellObject by 90 degrees
                cellObject.transform.Rotate(0, Random.Range(0, 4) * 90, 0);

                // Get the Cell component of the cellObject
                Cell cell = cellObject.GetComponent<Cell>();
                cell.Setup(gameEngine);
                cell.x = x;
                cell.y = y;
                
                // Set the cell in the _cells array
                _corralCells[x, y] = cellObject;
            }
        }
        
        // Create the "pf_Cell" prefab in every send immediate cell, above the corral, separated by a gap
        for (int x = 0; x < gameEngine.config.corralWidth; x++)
        {
            for (int y = 0; y < gameEngine.config.corralHeight; y++)
            {
                // Instantiate the prefab // Set the cellObject's parent to this transform
                GameObject cellObject = Instantiate(RandomUtils.GetRateRandomItem(gameEngine.config.grassCellPrefabs), transform, true);

                // Set the cellObject's position to the current x and y, plus the width of the board, plus the gap
                cellObject.transform.position = new Vector3(x + gameEngine.config.boardWidth + gameEngine.config.corralGap, 0, y + gameEngine.config.corralHeight + gameEngine.config.corralGap);

                // Randomly rotate the cellObject by 90 degrees
                cellObject.transform.Rotate(0, Random.Range(0, 4) * 90, 0);

                // Get the Cell component of the cellObject
                Cell cell = cellObject.GetComponent<Cell>();
                cell.Setup(gameEngine);
                cell.x = x;
                cell.y = y;
                
                // Set the cell in the _cells array
                _immediateSendCells[x, y] = cellObject;
            }
        }
    }

    private void CreateUIOnlyFenceAroundCorral()
    {
        // Put a little extra ui thing // TODO maybe put in UI Manager
        GameObject woodStructure = Resources.Load<GameObject>("Cells/Cosmetics/woodStructure");
        // instantiate the wood structure, in the middle of the corral
        GameObject woodStructureObject = Instantiate(woodStructure, transform, true);

        woodStructureObject.transform.position =
            new Vector3(gameEngine.config.boardWidth + gameEngine.config.corralGap, 0, 0) + new Vector3(.5f, 0, .5f); // .5f is for the cell size
        
        // Scale it to fit the corral
        woodStructureObject.transform.localScale = new Vector3(gameEngine.config.corralWidth, 1, gameEngine.config.corralHeight);
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
        _mainBoardCells[oldCell.x, oldCell.y] = newCell.GetGameObject();
        
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
        if (cell.IsBuildable() && gameEngine.board.IsCellInMainBoard(cell)) // We shouldn't build in the corral or enemy board
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
