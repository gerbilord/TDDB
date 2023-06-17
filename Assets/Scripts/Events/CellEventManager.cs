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
    
    // Create event for when a cell is hovered over
    public delegate void CellMouseEnter(ICell cell);
    public event CellMouseEnter OnCellMouseEntered;
    public void CellMouseEntered(ICell cell)
    {
        /* This may cause issues in the future, but for now it's fine.
         * 
         * Example, we are on a card, and on a cell. This will get triggered.
         * We exit the card, but are still on the same cell. This will not get triggered when we enter the cell again.
         * Either 1. Make things listening to the event check for this. <-- probably easier
         * Or 2. Make the event trigger when we leave the UI element, if we are hovered over a cell, trigger.
        */
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        OnCellMouseEntered?.Invoke(cell);
    }
    
    // Create event for when a cell is unhovered over
    public delegate void CellMouseExit(ICell cell);
    public event CellMouseExit OnCellMouseExited;
    public void CellMouseExited(ICell cell)
    {
        /* We probably don't want this check for now.
         * Example, if we hover over a cell next to a card ui and highlight it, we still want the cell to be unhighlighted when we hover over the card.
         * 
         * This has a similar problem to CellMouseEntered
         * Either 1. Make this trigger when we enter a UI element and are on a card.
         * Or 2. Make a "OnUIEnter" and "OnUIExit" event, and have the UI manager deal with it. <-- probably easier
         */
        /*if (EventSystem.current.IsPointerOverGameObject()) 
        {
            return;
        }*/
        OnCellMouseExited?.Invoke(cell);
    }
    
}
