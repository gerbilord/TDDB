using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

// create enum for card return type
public enum CardPlayResult
{
    SUCCESS, // We tried to play the card, and it was successful
    FAIL,    // We tried to play the card, and it was not successful
    IGNORE   // We did not try to do anything
}
public class CardBehavior : MonoBehaviour, IPointerClickHandler, ICard
{
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
            if (result == CardPlayResult.FAIL) // Play the effect on the cell, and check the return value
            {
                // if playEffect.Play returns false, then the card was not played successfully.
                // Return false to indicate that the card was not played successfully.
                return CardPlayResult.FAIL;
            }
        }
        return ProcessResults(results);
    }

    private CardPlayResult ProcessResults(List<CardPlayResult> cardPlayResults)
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
        List<CardPlayResult> results = new List<CardPlayResult>();
        
        foreach (IPlayEffects playEffect in playEffects)
        {
            CardPlayResult result = playEffect.UI_OnCellClicked(cell);
            results.Add(result);
        }
        
        return ProcessResults(results);
    }
    
    public CardPlayResult UI_OnCellEntered(ICell cell)
    {
        List<CardPlayResult> results = new List<CardPlayResult>();
        
        foreach (IPlayEffects playEffect in playEffects)
        {
            CardPlayResult result = playEffect.UI_OnCellEntered(cell);
            results.Add(result);
        }
        
        return ProcessResults(results);
    }

    public CardPlayResult UI_OnCellExited(ICell cell)
    {
        List<CardPlayResult> results = new List<CardPlayResult>();
        
        foreach (IPlayEffects playEffect in playEffects)
        {
            CardPlayResult result = playEffect.UI_OnCellExited(cell);
            results.Add(result);
        }
        
        return ProcessResults(results);
    }
    
    public CardPlayResult UI_OnCardSelected()
    {
        List<CardPlayResult> results = new List<CardPlayResult>();
        
        foreach (IPlayEffects playEffect in playEffects)
        {
            CardPlayResult result = playEffect.UI_OnCardSelected();
            results.Add(result);
        }
        
        return ProcessResults(results);
    }
    
    public CardPlayResult UI_OnCardDeselected()
    {
        List<CardPlayResult> results = new List<CardPlayResult>();
        
        foreach (IPlayEffects playEffect in playEffects)
        {
            CardPlayResult result = playEffect.UI_OnCardDeselected();
            results.Add(result);
        }
        
        return ProcessResults(results);
    }
}
