using UnityEngine;

public class UIManager
{

    private GameObject _selectedCell; 
    
    public UIManager()
    {
        GlobalVariables.eventManager.OnCellClick += OnCellClicked;
    }

    ~UIManager()
    {
        GlobalVariables.eventManager.OnCellClick -= OnCellClicked;
    }
    
    // OnCellClicked event
    public void OnCellClicked(ICell cell)
    {
        GlobalVariables.gameEngine.board.PlaceTowerOn(cell);

        GameObject oldCell = _selectedCell;
        _selectedCell = cell.GetGameObject();
        
        if(oldCell != null)
        {
            ToggleHighlightCellAndObjects(oldCell.GetComponent<ICell>(), false);
        }
        
        ToggleHighlightCellAndObjects(cell, true);
    }

    public void ToggleHighlightCellAndObjects(ICell cell, bool toggle)
    {
        cell.GetGameObject().GetComponent<Highlighter>().ToggleHighlight(toggle);
        foreach (GameObject gameObject in cell.occupyingGameObjects)
        {
            gameObject.GetComponent<Highlighter>().ToggleHighlight(toggle);
        }
    }

}
