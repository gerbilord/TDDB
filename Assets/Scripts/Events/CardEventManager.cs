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
    
    // Create event for when a card is added to the hand
    public delegate void CardAddToDeck(ICard card);
    public event CardAddToDeck OnCardAddedToDeck;
    public void CardAddedToDeck(ICard card)
    {
        OnCardAddedToDeck?.Invoke(card);
    }
    
    // Create event for when a card is clicked
    public delegate void CardClick(ICard card);
    public event CardClick OnCardClick;
    public void CardClicked(ICard card)
    {
        OnCardClick?.Invoke(card);
    }
    
    // Create an event for when a card is played
    public delegate void CardPlay(ICard card);
    public event CardPlay OnCardPlay;
    public void CardPlayed(ICard card)
    {
        OnCardPlay?.Invoke(card);
    }
    
    // Create event for when the handSize is changed
    public delegate void HandSizeChange();
    public event HandSizeChange OnHandSizeChange;
    public void HandSizeChanged()
    {
        OnHandSizeChange?.Invoke();
    }
}
