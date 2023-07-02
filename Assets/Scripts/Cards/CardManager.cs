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
    public List<ICard> cardsUsedThisTurn = new List<ICard>();
    public List<ICard> cardsInCorral = new List<ICard>();
    
    
    public List<ICard> cardsInShop = new List<ICard>();
    
    GameObject discardGameObjectParent; // Gameobjects are set to children of these to appear nicely organized in the unity hierarchy editor
    GameObject corralGameObjectParent; 
    GameObject deckGameObjectParent;
    GameObject shopGameObjectParent;

    // Constructor
    public CardManager(IGameEngine gameEngine)
    {
        this.gameEngine = gameEngine;

        // Create a empty game object
        discardGameObjectParent = new GameObject("DiscardPile"); // TODO should this be in the UI?
        deckGameObjectParent = new GameObject("Deck");
        corralGameObjectParent = new GameObject("CorralCards");
        shopGameObjectParent = new GameObject("Shop");
        
        GlobalVariables.eventManager.cardEventManager.OnCardPlay += OnCardPlayed;
    }
    
    // Destructor
    ~CardManager()
    {
        GlobalVariables.eventManager.cardEventManager.OnCardPlay -= OnCardPlayed;
    }

    public void DiscardHand()
    {
        foreach (var card in cardsInHand)
        {
            cardsInDiscard.Add(card);
            card.GetGameObject().transform.SetParent(discardGameObjectParent.transform);
        }

        cardsInHand.Clear();
    }
    
    public void RerollShop()
    {
        // Destroy all objects in the shop
        foreach (ICard card in cardsInShop)
        {
            GameObject.Destroy(card.GetGameObject());
        }
        cardsInShop.Clear();
        LoadShop();
        GlobalVariables.eventManager.cardEventManager.ShopRerolled();
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

    public void LoadShop()
    {
        // iterate as many times as the shop size
        for (int i = 0; i < gameEngine.config.shopSize; i++)
        {
            // Get a random card from the lvl 1 cards
            ICard newShopCard = RandomUtils.GetRateRandomItem(gameEngine.config.shopCardPrefabs_1).makeCard(); 

            // Add it to the shop
            cardsInShop.Add(newShopCard);
            // Set the card's parent to the shop, this makes it visible
            newShopCard.GetGameObject().transform.SetParent(shopGameObjectParent.transform);
            
            GlobalVariables.eventManager.cardEventManager.CardAddedToShop(newShopCard);
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
        // Cards can currently be played from the hand or the shop. 
        // Just make sure the card is removed from both before we send it elsewhere.
        cardsInHand.Remove(card);
        cardsInShop.Remove(card);

        switch (card.GetWhereToSendThisCard())
        {
            // case for each enum value
            case CardWhereToSend.CORRAL:
                cardsInCorral.Add(card);
                card.GetGameObject().transform.SetParent(corralGameObjectParent.transform);
                break;
            case CardWhereToSend.DEFAULT:
                cardsUsedThisTurn.Add(card);
                card.GetGameObject().transform.SetParent(discardGameObjectParent.transform); // Set the card's parent to the discard pile, this makes it hidden
                break;
            default:
                Debug.LogError("CardWhereToSend enum value not handled in CardManager.OnCardPlayed");
                break;
        }

        GlobalVariables.eventManager.cardEventManager.HandSizeChanged();
    }

    public void MoveUsedCardsToDiscard()
    {
        foreach (var card in cardsUsedThisTurn)
        {
            // set card parent to discard parent
            card.GetGameObject().transform.SetParent(discardGameObjectParent.transform);
        }

        cardsInDiscard.AddRange(cardsUsedThisTurn);
        cardsUsedThisTurn.Clear();
    }
    
    public void MoveCorralCardsToDiscard()
    {
        foreach (var card in cardsInCorral)
        {
            // set card parent to discard parent
            card.GetGameObject().transform.SetParent(discardGameObjectParent.transform);
        }

        cardsInDiscard.AddRange(cardsInCorral);
        cardsInCorral.Clear();
    }
}
