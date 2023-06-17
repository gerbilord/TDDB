using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEventManager
{
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
