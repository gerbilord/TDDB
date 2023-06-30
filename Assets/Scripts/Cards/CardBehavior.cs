using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

// create enum for card return type
public enum CardPlayResult
{
    SUCCESS, // We tried to play the card, and it was successful
    FAIL,    // We tried to play the card, and it was not successful
    IGNORE_BUT_STOP_OTHER_EFFECTS,   // We did not try to do anything, but don't let other effects play.
    IGNORE // Let other effects do things. (For example, ui highlighting, some effects may not care to do anything on hover)
}

public enum CardWhereToSend
{
    DEFAULT, // Send the card to the used pile, but allow others to send elsewhere.
    CORRAL,
}

public class CardBehavior : MonoBehaviour, IPointerClickHandler, ICard
{
    [DoNotSerialize]
    public IGameEngine gameEngine { get; set; }

    public List<IPlayEffects> playEffects { get; set; }

    public void OnPointerClick (PointerEventData eventData)
    {
        GlobalVariables.eventManager.cardEventManager.CardClicked(this);
    }
    
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
    
    public CardPlayResult Play(ICell cell)
    {
        List<CardPlayResult> results = new List<CardPlayResult>();
        
        foreach (IPlayEffects playEffect in playEffects)
        {
            CardPlayResult result = playEffect.Play(cell);
            results.Add(result);
            if (result == CardPlayResult.IGNORE_BUT_STOP_OTHER_EFFECTS)
            {
                return CardPlayResult.IGNORE_BUT_STOP_OTHER_EFFECTS;
            }
                
            if (result == CardPlayResult.FAIL) // Play the effect on the cell, and check the return value
            {
                // if playEffect.Play returns false, then the card was not played successfully.
                // Return false to indicate that the card was not played successfully.
                return CardPlayResult.FAIL;
            }
        }

        return ProcessResultsAndSendEvent(results);
    }
    
    public CardPlayResult CallEffectsAndGetResult<T>(ICell cell, Func<T, ICell, CardPlayResult> action) where T : IPlayEffects
    {
        List<CardPlayResult> results = new List<CardPlayResult>();
    
        foreach (T playEffect in playEffects.OfType<T>())
        {
            CardPlayResult result = action(playEffect, cell);
            results.Add(result);
            if (result == CardPlayResult.IGNORE_BUT_STOP_OTHER_EFFECTS)
            {
                return CardPlayResult.IGNORE; // The top level doesn't need to know about our stop effects.
            }
            
            if (result == CardPlayResult.FAIL) // Play the effect on the cell, and check the return value
            {
                // if action returns false, then the card was not played successfully.
                // Return false to indicate that the card was not played successfully.
                return CardPlayResult.FAIL;
            }
        }

        return ProcessResultsAndSendEvent(results);
    }
    
    public CardPlayResult CallEffectsAndGetResult<T>(Func<T, CardPlayResult> action) where T : IPlayEffects
    {
        List<CardPlayResult> results = new List<CardPlayResult>();
    
        foreach (T playEffect in playEffects.OfType<T>())
        {
            CardPlayResult result = action(playEffect);
            results.Add(result);
            if (result == CardPlayResult.IGNORE_BUT_STOP_OTHER_EFFECTS)
            {
                return CardPlayResult.IGNORE; // The top level doesn't need to know about our stop effects.
            }
            
            if (result == CardPlayResult.FAIL) // Play the effect on the cell, and check the return value
            {
                // if action returns false, then the card was not played successfully.
                // Return false to indicate that the card was not played successfully.
                return CardPlayResult.FAIL;
            }
        }

        return ProcessResultsAndSendEvent(results);
    }

    public CardWhereToSend GetWhereToSendThisCard()
    {
        foreach (var effect in playEffects)
        {
            if(effect.GetWhereToSendThisCard() != CardWhereToSend.DEFAULT)
                return effect.GetWhereToSendThisCard();
        }

        return CardWhereToSend.DEFAULT;
    }

    // This function sucks
    private CardPlayResult ProcessResultsAndSendEvent(List<CardPlayResult> cardPlayResults)
    {
        // Create a set
        HashSet<CardPlayResult> cardPlayResultSet = new HashSet<CardPlayResult>();
        foreach (CardPlayResult cardPlayResult in cardPlayResults)
        {
            cardPlayResultSet.Add(cardPlayResult);
        }
        
        // check if the set contains only one element
        if (cardPlayResultSet.Count == 1)
        {
            // if so, return that element. (This is a hacky conversion to get the only element in the Set).
            CardPlayResult result = cardPlayResultSet.ToList()[0];

            if (result == CardPlayResult.SUCCESS)
            {
                GlobalVariables.eventManager.cardEventManager.CardPlayed(this);
            }
            
            return result;
        }
        
        // Debug info about the effects
        foreach (CardPlayResult cardPlayResult in cardPlayResults)
        {
            Debug.Log(cardPlayResult);
        }
       
        // Error: multiple results
        Debug.LogError("Multiple results from card play effects");

        // Throw error
        throw new Exception("Multiple results from card play effects");
    }

    public CardPlayResult UI_OnCellClicked(ICell cell)
    {
        return CallEffectsAndGetResult<IPlayEffects>(cell, (playEffect, cell) => playEffect.UI_OnCellClicked(cell));
    }
    
    public CardPlayResult UI_OnCellEntered(ICell cell)
    {
       return CallEffectsAndGetResult<IPlayEffects>(cell, (playEffect, cell) => playEffect.UI_OnCellEntered(cell));
    }

    public CardPlayResult UI_OnCellExited(ICell cell)
    {
        return CallEffectsAndGetResult<IPlayEffects>(cell, (playEffect, cell) => playEffect.UI_OnCellExited(cell));
    }
    
    public CardPlayResult UI_OnCardSelected()
    {
        return CallEffectsAndGetResult<IPlayEffects>((playEffect) => playEffect.UI_OnCardSelected());
    }
    
    public CardPlayResult UI_OnCardDeselected()
    {
        return CallEffectsAndGetResult<IPlayEffects>((playEffect) => playEffect.UI_OnCardDeselected());
    }
}
