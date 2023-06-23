using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager
{
    public List<ICard> cardsInHand = new List<ICard>();

    public void LoadDeck()
    {
        List<CardPreset> deck = GlobalVariables.config.DeckPreset.cards;
        foreach (CardPreset cardPreset in deck)
        {
            ICard card = cardPreset.makeCard();
            cardsInHand.Add(card);
            GlobalVariables.eventManager.cardEventManager.CardAddedToHand(card);
        }
    }
}
