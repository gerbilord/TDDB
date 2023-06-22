using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager
{
    public List<ICard> cardsInHand = new List<ICard>();
    
    public CardManager()
    {
        
    }

    public void LoadDeck()
    {
        List<CardPreset> Deck = GlobalVariables.config.DeckPreset.cards;
        foreach (CardPreset cardPreset in Deck)
        {
            GameObject cardObject = GameObject.Instantiate(cardPreset.cardPrefab);
            ICard card = cardObject.GetComponent<ICard>();
            card.towerPreset = cardPreset.towerPreset;
            cardsInHand.Add(card);
            GlobalVariables.eventManager.cardEventManager.CardAddedToHand(card);
        }
    }
}
