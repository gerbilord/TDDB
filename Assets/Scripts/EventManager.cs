using UnityEngine;
using UnityEngine.EventSystems;

public class EventManager
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
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        OnCellClick?.Invoke(cell);
    }
    
    // Create event for when a card is added to the hand
    public delegate void CardAddToHand(ICard card);
    public event CardAddToHand OnCardAddedToHand;
    
    public void CardAddedToHand(ICard card)
    {
        OnCardAddedToHand?.Invoke(card);
    }
    
    // Create event for when a card is clicked
    public delegate void CardClick(ICard card);
    public event CardClick OnCardClick;
    
    public void CardClicked(ICard card)
    {
        OnCardClick?.Invoke(card);
    }
}
