using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
}
