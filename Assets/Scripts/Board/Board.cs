using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // Field of 2d List of Cells
    private GameObject[,] _cells;
    private GameEngine _gameEngine;
    
    void Start()
    {
        // Access the pf_GameEngine on the scene
        _gameEngine = FindObjectOfType<GameEngine>();

        CreateCellGrid();
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
                GameObject cellObject = Instantiate(Resources.Load<GameObject>("pf_Cell"), transform, true);

                // Set the cellObject's position to the current x and y
                cellObject.transform.position = new Vector3(x, y, 0);

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
