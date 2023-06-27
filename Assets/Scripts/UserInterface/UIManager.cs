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
    
    private GameObject _playerHealth;
    private GameObject _playerIncome;
    private GameObject _playerMoney;
    
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
        LoadDeckAndDiscardUI();
        LoadPlayerStatsUI();
        LoadCameraIcon();
    }

    private void LoadCameraIcon()
    {
        GameObject cameraIcon = GameObject.Instantiate(Resources.Load<GameObject>("UI/MiscIcons/CameraSwitchIcon"), _canvas.transform, true);
        cameraIcon.transform.position = new Vector3(Screen.width - GraphicsUtils.GetWidthOf2d(cameraIcon) / 2, Screen.height - GraphicsUtils.GetHeightOf2d(cameraIcon) / 2, 0);
    }

    private void LoadPlayerStatsUI()
    {
        _playerHealth = GameObject.Instantiate(Resources.Load<GameObject>("UI/Player/Health"), _canvas.transform, true);
        _playerIncome = GameObject.Instantiate(Resources.Load<GameObject>("UI/Player/Income"), _canvas.transform, true);
        _playerMoney = GameObject.Instantiate(Resources.Load<GameObject>("UI/Player/Money"), _canvas.transform, true);
        
        Vector3 topRight = new Vector3(Screen.width, Screen.height, 0);
        
        float magicNumber_yOffsetExtra = 5f;
        float yOffset = -GraphicsUtils.GetHeightOf2d(_playerIncome) - magicNumber_yOffsetExtra;
        
        // place _playerHealth in the top right corner of the screen
        _playerHealth.transform.position = topRight + new Vector3(-GraphicsUtils.GetWidthOf2d(_playerHealth) / 2, -GraphicsUtils.GetHeightOf2d(_playerHealth) / 2, 0);
        _playerIncome.transform.position = _playerHealth.transform.position + new Vector3(0, yOffset, 0);
        _playerMoney.transform.position = _playerIncome.transform.position + new Vector3(0, yOffset, 0);

        UpdatePlayerStatsUI();
    }
    
    public void UpdatePlayerStatsUI()
    {
        // get text from the text mesh pro from _playerHealth
        TextMeshProUGUI textMeshHealth = _playerHealth.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI textMeshIncome = _playerIncome.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI textMeshMoney = _playerMoney.GetComponentInChildren<TextMeshProUGUI>();

        textMeshHealth.text = GlobalVariables.gameEngine.player.health.ToString();
        textMeshIncome.text = GlobalVariables.gameEngine.player.income.ToString();
        textMeshMoney.text = GlobalVariables.gameEngine.player.money.ToString();
    }

    private void LoadDeckAndDiscardUI()
    {
        Vector3 bottomLeft = new Vector3(0, 0, 0);

        // Instantiate the deck and card back
        _deckCardBack = GameObject.Instantiate(Resources.Load<GameObject>("Cards/UIOnlyCards/deck_card_back"), _canvas.transform, true);
        _discardCardBack = GameObject.Instantiate(Resources.Load<GameObject>("Cards/UIOnlyCards/discard_card_back"), _canvas.transform, true);

        // Just use one scale for both cards, assume they are the same size
        Vector3 scale = _discardCardBack.transform.localScale;
        float magicNumber_xOffSetExtra = 20f;
        float magicNumber_yOffsetExtra = 20f;
        float magicNumber_SpaceBetweenCards = 5f;

        float cardWidth = GraphicsUtils.GetWidthOf2d(_deckCardBack);
        float cardHeight = GraphicsUtils.GetHeightOf2d(_deckCardBack);

        // put the _discardCardBack in the bottom left corner, and put the deck card back above it
        _discardCardBack.transform.position = bottomLeft + new Vector3(cardWidth / 2 + magicNumber_xOffSetExtra,
            cardHeight / 2 + magicNumber_yOffsetExtra, 0);
        _deckCardBack.transform.position = _discardCardBack.transform.position +
                                           new Vector3(0, cardHeight + magicNumber_SpaceBetweenCards, 0);
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
            float magicNumberExtraPaddingX = 10f;

            var localScale = cardGameObject.transform.localScale;

            float cardWidth = GraphicsUtils.GetWidthOf2d(cardGameObject);
            float cardHeight = GraphicsUtils.GetHeightOf2d(cardGameObject);

            // Put the card game object at the bottom of the canvas UI, off set by the width of the card
            cardGameObject.transform.position = bottomCenter +
                                                new Vector3(
                                                    offsetMultiplier *
                                                    (cardWidth +
                                                     magicNumberExtraPaddingX),
                                                    cardHeight / 2 +
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

    public void SetupCamera()
    {
        int boardWidth = GlobalVariables.config.boardWidth;
        int boardHeight = GlobalVariables.config.boardHeight;

        // Calculate the center position of the board
        Vector3 boardCenter = GlobalVariables.gameEngine.board.CalculateBoardCenter();

        Camera mainCamera = GlobalVariables.gameEngine.mainCamera;
        
        // Set the camera perspective
        mainCamera.orthographic = false;

        // Calculate the distance from the board to the camera
        float distance = Mathf.Max(boardWidth, boardHeight) / (2f * Mathf.Tan(mainCamera.fieldOfView * 0.5f * Mathf.Deg2Rad));

        // Set the camera position to view from the top
        mainCamera.transform.position = new Vector3(boardCenter.x - distance/2, distance, boardCenter.z - distance/2);

        // Set the camera rotation to view from the top
        mainCamera.transform.rotation = Quaternion.Euler(70f, 0f, 0f);
    }

    public void ToggleCameraPerspective()
    {
        Camera mainCamera = GlobalVariables.gameEngine.mainCamera;
        if (mainCamera.transform.rotation == Quaternion.Euler(50f, 40f, 0f))
        {
            mainCamera.transform.rotation = Quaternion.Euler(70f, 0f, 0f);
        }
        else
        {
            mainCamera.transform.rotation = Quaternion.Euler(50f, 40f, 0f);
        }
        
    }
}
