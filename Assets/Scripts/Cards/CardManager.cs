using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager
{
    public List<GameObject> cardsInHand;
    
    public CardManager()
    {
        cardsInHand = new List<GameObject>();
        AddCardToHand(GlobalVariables.config.towerCardPrefab);
    }

    public void AddCardToHand(GameObject cardPrefab)
    {

        // Instantiate cardPrefab
        GameObject card = GameObject.Instantiate(cardPrefab);

        cardsInHand.Add(card);
        GlobalVariables.eventManager.CardAddedToHand(card.GetComponent<ICard>());
    }
}
