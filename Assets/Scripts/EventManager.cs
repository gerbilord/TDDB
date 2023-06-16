public class EventManager
{
    // Create event for when a cell changes
    public delegate void CellChange(ICell oldCell, ICell newCell);
    public event CellChange OnCellChange;
    
    
    public void CellChanged(ICell oldCell, ICell newCell)
    {
        if (OnCellChange != null)
        {
            OnCellChange(oldCell, newCell);
        }
    }
    
    // Create event for when a cell is clicked
    public delegate void CellClick(ICell cell);
    public event CellClick OnCellClick;
    
    public void CellClicked(ICell cell)
    {
        if (OnCellClick != null)
        {
            OnCellClick(cell);
        }
    }
    
}
