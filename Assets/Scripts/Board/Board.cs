using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Prefab to use as the cells
    private GameObject _pfCell;

    // Field of 2d List of Cells
    private GameObject[,] _cells;
    private GameEngine _gameEngine;
    private Camera _mainCamera;

    private List<GameObject> _path;

    void Start()
    {
        // Access the pf_GameEngine on the scene
        _gameEngine = FindObjectOfType<GameEngine>();
        _mainCamera = _gameEngine.mainCamera;
        
        // Set the _pfCell from _gameEngine
        _pfCell = _gameEngine.pfCell;

        CreateCellGrid();
        CreatePath();

        // Adjust camera to view the entire board
        int boardWidth = _gameEngine.boardWidth;
        int boardHeight = _gameEngine.boardHeight;

        SetupCamera(boardWidth, boardHeight);
        
        // Update the render of all cells
        UpdateCellRender();
        
    }

    private void SetupCamera(int boardWidth, int boardHeight)
    {
        // Calculate the center position of the board
        Vector3 boardCenter = CalculateBoardCenter();

        // Set the camera position to view from the top
        // _mainCamera.transform.position = new Vector3(boardCenter.x, boardWidth + boardHeight, boardCenter.z);
        
        // Calculate the distance from the board to the camera
        float distance = Mathf.Max(boardWidth, boardHeight) / (2f * Mathf.Tan(_mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad));

        // Set the camera position to view from the top
        _mainCamera.transform.position = new Vector3(boardCenter.x - distance/2, distance, boardCenter.z - distance/2);

        // Set the camera rotation to view from the top
        _mainCamera.transform.rotation = Quaternion.Euler(50f, 40f, 0f);

        // Set the camera perspective
        _mainCamera.orthographic = false;
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
    
    private void UpdateCellRender()
    {
        // Call update render on each cell
        foreach (GameObject cellObject in _cells)
        {
            // Get the Cell component of the cellObject
            Cell cell = GetCell(cellObject);

            // Call the UpdateRender method on the cell
            cell.UpdateRender();
        }
    }
    

    private void CreatePath()
    {
        // Create a path
        _path = new List<GameObject>();

        // choose a random y within boardHeight
        int y = Random.Range(0, _gameEngine.boardHeight);

        // Create a simple path from the left side to the right side of the board
        for (int x = 0; x < _gameEngine.boardWidth; x++)
        {
            // Get the cell at the current x and 0
            Cell cell = GetCell(_cells[x, y]);

            // Add the cell to the path
            _path.Add(cell.gameObject);

            // Set the cell's isPath to true
            cell.isPath = true;
        }
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
                GameObject cellObject = Instantiate(_pfCell, transform, true);

                // Set the cellObject's position to the current x and y
                cellObject.transform.position = new Vector3(x, 0, y);

                // Get the Cell component of the cellObject
                Cell cell = cellObject.GetComponent<Cell>();
                cell.x = x;
                cell.y = y;
                cell.isBuildable = true;

                // Set the cell in the _cells array
                _cells[x, y] = cellObject;
            }
        }
    }

    // Given a GameObject, get the cell script at the GameObject's position
    public Cell GetCell(GameObject gameObject)
    {
        // Get the Cell component of the gameObject
        Cell cell = gameObject.GetComponent<Cell>();

        // Return the cell
        return cell;
    }
}
