using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager
{

    private Canvas _canvas;
    private GameObject _selectedCell;
    private ICard _selectedCard; // Selected card is responsible for custom UI. (Such as a tower preview when hovering over a cell)
    private GameObject _deckCardBack;
    private GameObject _discardCardBack;
    
    public UIManager()
    {
        LoadBasicUI();
        
        GlobalVariables.eventManager.cellEventManager.OnCellClick += OnCellClicked;
        GlobalVariables.eventManager.cardEventManager.OnCardAddedToHand += OnCardAddedToHand;
        GlobalVariables.eventManager.cardEventManager.OnCardClick += OnCardClicked;
        GlobalVariables.eventManager.cellEventManager.OnCellMouseEntered += OnCellEntered;
        GlobalVariables.eventManager.cellEventManager.OnCellMouseExited += OnCellExited;
        GlobalVariables.eventManager.cardEventManager.OnCardPlay += OnCardPlayed;
        GlobalVariables.eventManager.cardEventManager.OnHandSizeChange += OnHandSizeChanged;
    }

    ~UIManager()
    {
        GlobalVariables.eventManager.cellEventManager.OnCellClick -= OnCellClicked;
        GlobalVariables.eventManager.cardEventManager.OnCardAddedToHand -= OnCardAddedToHand;
        GlobalVariables.eventManager.cardEventManager.OnCardClick -= OnCardClicked;
        GlobalVariables.eventManager.cellEventManager.OnCellMouseEntered -= OnCellEntered;
        GlobalVariables.eventManager.cellEventManager.OnCellMouseExited -= OnCellExited;
        GlobalVariables.eventManager.cardEventManager.OnCardPlay -= OnCardPlayed;
        GlobalVariables.eventManager.cardEventManager.OnHandSizeChange += OnHandSizeChanged;
    }

    private void LoadBasicUI()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        Vector3 bottomLeft = new Vector3(0, 0, 0);
        Vector3 bottomRight = new Vector3(Screen.width, 0, 0);
        
        
        // Instantiate the deck card back
        _deckCardBack = GameObject.Instantiate(Resources.Load<GameObject>("Cards/UIOnlyCards/deck_card_back"));
        _deckCardBack.transform.SetParent(_canvas.transform);
        
        _discardCardBack = GameObject.Instantiate(Resources.Load<GameObject>("Cards/UIOnlyCards/discard_card_back"));
        _discardCardBack.transform.SetParent(_canvas.transform);
        
        // put the _deckCardBack in the bottom left corner using bottomLeft, and offset by card width and height
        _deckCardBack.transform.position = bottomLeft + new Vector3(_deckCardBack.GetComponent<RectTransform>().rect.width, _deckCardBack.GetComponent<RectTransform>().rect.height, 0);
        
        // put the _discardCardBack in the bottom right corner using bottomRight
        _discardCardBack.transform.position = bottomRight + new Vector3(-_discardCardBack.GetComponent<RectTransform>().rect.width, _discardCardBack.GetComponent<RectTransform>().rect.height, 0);
    }

    private void UpdateDeckTextUI()
    {
        // get the text from the deck card back
        TextMeshProUGUI deckText = _deckCardBack.GetComponentInChildren<TextMeshProUGUI>();
        deckText.text = GlobalVariables.gameEngine.cardManager.cardsInDeck.Count.ToString();
        
        // get the text from the discard card back
        TextMeshProUGUI discardText = _discardCardBack.GetComponentInChildren<TextMeshProUGUI>();
        discardText.text = GlobalVariables.gameEngine.cardManager.cardsInDiscard.Count.ToString();
    }

    public void OnCellEntered(ICell cell)
    {
        if (_selectedCard != null)
        {
            _selectedCard.UI_OnCellEntered(cell);
        }
    }

    public void OnCellExited(ICell cell)
    {
        if(_selectedCard != null)
        {
            _selectedCard.UI_OnCellExited(cell);
        }
    }
    
    public void OnCellClicked(ICell cell)
    {
        if (_selectedCard != null)
        {
             _selectedCard.UI_OnCellClicked(cell);
        }
        else
        {
            GlobalVariables.gameEngine.board.GetAllCells().ForEach(anICell => ToggleHighlightCellAndObjects(anICell, false));
            ToggleHighlightCellAndObjects(cell, true);
        }
        
    }

    public void ToggleHighlightCellAndObjects(ICell cell, bool toggle)
    {
        cell.GetGameObject().GetComponent<IHighlighter>().ToggleHighlight(toggle);
        foreach (GameObject gameObject in cell.occupyingGameObjects)
        {
            gameObject.GetComponent<IHighlighter>().ToggleHighlight(toggle);
        }
    }
    
    public void ToggleHighlightCellAndObjects(ICell cell, bool toggle, Color color, float intensity)
    {
        cell.GetGameObject().GetComponent<IHighlighter>().ToggleHighlight(toggle, color, intensity);
        foreach (GameObject gameObject in cell.occupyingGameObjects)
        {
            gameObject.GetComponent<IHighlighter>().ToggleHighlight(toggle, color, intensity);
        }
    }

    public void ToggleHighlight(GameObject objectToHighlight, bool toggle)
    {
        objectToHighlight.GetComponent<IHighlighter>().ToggleHighlight(toggle);
    }

    // OnCardAddedToHand event
    public void OnCardAddedToHand(ICard card)
    {
    }

    private void ResetCardsInHandPosition()
    {
        // Get bottom center of the UI canvas
        Vector3 bottomCenter = new Vector3(Screen.width / 2f, 0, 0);

        // Get the cards in hand
        GlobalVariables.gameEngine.cardManager.cardsInHand.ForEach(aCard =>
        {
            // Get the card game object
            GameObject cardGameObject = aCard.GetGameObject();

            int totalCards = GlobalVariables.gameEngine.cardManager.cardsInHand.Count;
            int cardIndex = GlobalVariables.gameEngine.cardManager.cardsInHand.IndexOf(aCard);

            float offsetMultiplier = cardIndex - (totalCards / 2f) + 0.5f;
            float magicNumberExtraPaddingY = 20f;
            float magicNumberExtraPaddingX = 0f; // 30f;

            // Put the card game object at the bottom of the canvas UI, off set by the width of the card
            cardGameObject.transform.position = bottomCenter +
                                                new Vector3(
                                                    offsetMultiplier *
                                                    (cardGameObject.GetComponent<RectTransform>().rect.width +
                                                     magicNumberExtraPaddingX),
                                                    cardGameObject.GetComponent<RectTransform>().rect.height / 2 +
                                                    magicNumberExtraPaddingY, 0);

            // Set the parent of the card game object to the canvas, this makes the card visible.
            cardGameObject.transform.SetParent(_canvas.transform);
        });
    }

    public void OnCardClicked(ICard cardClicked)
    {
        // Unhighlight all cells and objects.
        GlobalVariables.gameEngine.board.GetAllCells().ForEach(anICell => ToggleHighlightCellAndObjects(anICell, false));
        
        // Get both the old and new card game objects
        GameObject newCard = cardClicked.GetGameObject();
        GameObject oldCard = null;
        if (_selectedCard != null)
        {
            oldCard = _selectedCard.GetGameObject();
        }

        // If the old card is not null, unhighlight it.
        if (oldCard != null)
        {
            oldCard.GetComponent<IHighlighter>().ToggleHighlight(false);
        }
        
        // If the old card is the same as the new card, unselect it. (It will also be unhighlighted from the above code).
        if(oldCard == newCard)
        {
            oldCard.GetComponent<ICard>().UI_OnCardDeselected();
            _selectedCard = null;
            return;
        }

        // Since the new card is not the same as the old card, we need to unselect the old card.
        if (oldCard != null)
        {
            oldCard.GetComponent<ICard>().UI_OnCardDeselected();
        }
        
        // And select the new card.
        _selectedCard = newCard.GetComponent<ICard>();
        _selectedCard.UI_OnCardSelected();
        newCard.GetComponent<IHighlighter>().ToggleHighlight(true);
    }

    public void OnCardPlayed(ICard cardPlayed)
    {
        // turn off highlight of card
        cardPlayed.GetGameObject().GetComponent<IHighlighter>().ToggleHighlight(false);

        _selectedCard = null;
    }

    public void OnHandSizeChanged()
    {
        ResetCardsInHandPosition();
        UpdateDeckTextUI();
    }
}
