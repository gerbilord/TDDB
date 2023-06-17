using UnityEngine.EventSystems;

public class CellEventManager
{
    // Create event for when a cell changes
    public delegate void CellChange(ICell oldCell, ICell newCell);
    public event CellChange OnCellChange;
    public void CellChanged(ICell oldCell, ICell newCell)
    {
        OnCellChange?.Invoke(oldCell, newCell);
    }
    
    // Create event for when a cell is clicked
    public delegate void CellClick(ICell cell);
    public event CellClick OnCellClick;
    public void CellClicked(ICell cell)
    {
        if (EventSystem.current.IsPointerOverGameObject()) // If a cell was clicked through the ui (such as a card)
        {
            return; // Don't do anything
        }
        OnCellClick?.Invoke(cell);
    }
}
