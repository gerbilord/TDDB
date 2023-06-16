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
        GameObject oldCell = _selectedCell;
        _selectedCell = cell.GetGameObject();
        
        if(oldCell != null)
        {
            oldCell.gameObject.GetComponent<Highlighter>().ToggleHighlight(false);
        }
        
        _selectedCell.gameObject.GetComponent<Highlighter>().ToggleHighlight(true);
    }
    

}
