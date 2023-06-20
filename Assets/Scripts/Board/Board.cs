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
        CreatePath();
        SetupCamera();
        UpdateAllCellPrefabsToMatchType();
        StartCoroutine(GlobalVariables.gameEngine.waveManager.spawnWave(GlobalVariables.gameEngine.waves[0].waveCreeps, 5));
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

    private void SetupCamera()
    {
        int boardWidth = _gameEngine.boardWidth;
        int boardHeight = _gameEngine.boardHeight;

        // Calculate the center position of the board
        Vector3 boardCenter = CalculateBoardCenter();

        Camera mainCamera = _gameEngine.mainCamera;
        
        // Set the camera perspective
        mainCamera.orthographic = false;

        // Calculate the distance from the board to the camera
        float distance = Mathf.Max(boardWidth, boardHeight) / (2f * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad));

        // Set the camera position to view from the top
        mainCamera.transform.position = new Vector3(boardCenter.x - distance/2, distance, boardCenter.z - distance/2);

        // Set the camera rotation to view from the top
        mainCamera.transform.rotation = Quaternion.Euler(50f, 40f, 0f);
    }
    
    private Vector3 CalculateBoardCenter()
    {
        int totalCells = _gameEngine.boardWidth * _gameEngine.boardHeight;
        Vector3 sumPosition = Vector3.zero;

        for (int x = 0; x < _gameEngine.boardWidth; x++)
        {
            for (int y = 0; y < _gameEngine.boardHeight; y++)
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

    private void CreatePath()
    {
        // Create a path
        path = new List<GameObject>();

        // choose a random y within boardHeight
        int y = Random.Range(0, _gameEngine.boardHeight);

        // Create a simple path from the left side to the right side of the board
        for (int x = 0; x < _gameEngine.boardWidth; x++)
        {
            // Get the cell at the current x and y
            GameObject cellObject = _cells[x, y];
            path.Add(cellObject);

            // Set the cell type to dirt (path)
            ICell cell = GetCell(cellObject);
            cell.type = CellType.Dirt;
        }
        
        print(path[0]);
    }

    private void CreateCellGrid()
    {
        // Instantiate cells using _gameEngine's width and height
        _cells = new GameObject[_gameEngine.boardWidth, _gameEngine.boardHeight];

        // Create the "pf_Cell" prefab in every cell
        for (int x = 0; x < _gameEngine.boardWidth; x++)
        {
            for (int y = 0; y < _gameEngine.boardHeight; y++)
            {
                // Instantiate the prefab // Set the cellObject's parent to this transform
                GameObject cellObject = Instantiate(Utils.GetRateRandomItem(GlobalVariables.config.grassCellPrefabs), transform, true);

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
    private ICell GetCell(GameObject gameObject)
    {
        // Get the Cell component of the gameObject
        ICell cell = gameObject.GetComponent<ICell>();

        // Return the cell
        return cell;
    }
    
    // Create a function to call when a cell is changed
    private void OnCellChanged(ICell oldCell, ICell newCell)
    {
        // Change our reference
        _cells[oldCell.x, oldCell.y] = newCell.GetGameObject();
    }

    public void PlaceTowerOn(ICell cell)
    {
        if (cell.IsBuildable())
        {
            // Get the tower prefab
            GameObject towerPrefab = GlobalVariables.config.towerPrefab;
        
            // Get the cell gameObject
            GameObject cellObject = cell.GetGameObject();
        
            // Instantiate the tower prefab at the cell's position
            GameObject towerObject = Instantiate(towerPrefab, cellObject.transform.position, Quaternion.identity);
        
            // Set the tower's parent to the cell
            towerObject.transform.SetParent(cellObject.transform);

            // Add tower to occupying game objects
            cell.occupyingGameObjects.Add(towerObject);
        }
    }
}
