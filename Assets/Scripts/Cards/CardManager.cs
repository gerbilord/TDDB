using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardManager : IHasIGameEngine
{
    public IGameEngine gameEngine { get; set; }

    public List<ICard> cardsInHand = new List<ICard>();
    public List<ICard> cardsInDeck = new List<ICard>();
    public List<ICard> cardsInDiscard = new List<ICard>();
    
    GameObject discardGameObjectParent;
    GameObject deckGameObjectParent;

    // Constructor
    public CardManager(IGameEngine gameEngine)
    {
        this.gameEngine = gameEngine;

        // Create a empty game object
        discardGameObjectParent = new GameObject("DiscardPile"); // TODO should this be in the UI?
        deckGameObjectParent = new GameObject("Deck");
        
        GlobalVariables.eventManager.cardEventManager.OnCardPlay += OnCardPlayed;
    }
    
    // Destructor
    ~CardManager()
    {
        GlobalVariables.eventManager.cardEventManager.OnCardPlay -= OnCardPlayed;
    }

    public void LoadDeck()
    {
        List<CardPreset> deck = gameEngine.config.DeckPreset.cards;
        foreach (CardPreset cardPreset in deck)
        {
            ICard card = cardPreset.makeCard();
            cardsInDeck.Add(card);
            // Set the card's parent to the deck, this makes it hidden
            card.GetGameObject().transform.SetParent(deckGameObjectParent.transform);
            GlobalVariables.eventManager.cardEventManager.CardAddedToDeck(card);
        }
    }

    public void StartTurn()
    {
        DrawCards(gameEngine.config.startingCardAmount);
    }
    
    // Draw cards from the deck
    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (cardsInDeck.Count == 0)
            {
                // if discard also empty, break. We have no cards left to draw.
                if (cardsInDiscard.Count == 0)
                    break;

                // Otherwise, shuffle discard into deck, so we can draw more cards
                ShuffleDiscardIntoDeck();
            }
                
            ICard card = cardsInDeck[0];
            cardsInDeck.RemoveAt(0);
            cardsInHand.Add(card);
            GlobalVariables.eventManager.cardEventManager.CardAddedToHand(card);
        }
        GlobalVariables.eventManager.cardEventManager.HandSizeChanged();
    }

    private void ShuffleDiscardIntoDeck()
    {
        // Shuffle discard into deck
        cardsInDeck = cardsInDiscard;
        cardsInDiscard = new List<ICard>();
        
        cardsInDeck = cardsInDeck.OrderBy(card => Random.value).ToList();
    }
    
    public void OnCardPlayed(ICard card)
    {
        cardsInHand.Remove(card);
        cardsInDiscard.Add(card);
        GlobalVariables.eventManager.cardEventManager.HandSizeChanged();
        
        // Set the card's parent to the discard pile, this makes it hidden
        card.GetGameObject().transform.SetParent(discardGameObjectParent.transform); // Should this be the UI?
    }
}
