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
    private GameObject _usedCardBack;
    private GameObject _rerollButton;
    private GameObject _levelUpButton;
    private GameObject _nextTurnButton;
    private GameObject _nextTurnWithCorralButton;
    private GameObject _levelIndicator;
    
    private GameObject _playerHealth;
    private GameObject _playerIncome;
    private GameObject _playerMoney;
    
    private GameObject _enemyHealth;
    
    public UIManager()
    {
    }

    public void Setup()
    {
        LoadBasicUI();
        
        GlobalVariables.eventManager.cellEventManager.OnCellClick += OnCellClicked;
        GlobalVariables.eventManager.cardEventManager.OnCardAddedToHand += OnCardAddedToHand;
        GlobalVariables.eventManager.cardEventManager.OnCardClick += OnCardClicked;
        GlobalVariables.eventManager.cellEventManager.OnCellMouseEntered += OnCellEntered;
        GlobalVariables.eventManager.cellEventManager.OnCellMouseExited += OnCellExited;
        GlobalVariables.eventManager.cardEventManager.OnCardPlay += OnCardPlayed;
        GlobalVariables.eventManager.cardEventManager.OnHandSizeChange += OnHandSizeChanged;
        GlobalVariables.eventManager.cardEventManager.OnCardAddedToShop += OnCardAddedToShop;
        GlobalVariables.eventManager.cardEventManager.OnShopRerolled += OnShopRerolled;
    }

    ~UIManager()
    {
        // TODO FIX THESE TO INCLUDE ALL THE ABOVE ONES
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
        LoadNextTurnButtonsUI();
        LoadLevelIndicatorUI();
        LoadDeckAndDiscardUI();
        LoadPlayerStatsUI();
        LoadEnemyStatsUI();
        UpdateStatsUI();
        LoadCameraIcon();
        ResetCardsInHandPosition();
        UpdateDeckTextUI();
        UpdateLevelIndicatorUI();
        ResetCardsInShopPosition();
        LoadShopStaticUI();
        UpdateCreepSendAmountUI();
        UpdateShopButtonTextUI();
    }

    private void LoadLevelIndicatorUI()
    {
        // Get middle top of screen;
        Vector3 middleTop = new Vector3(Screen.width / 2f, Screen.height, 0); 

        _levelIndicator = GameObject.Instantiate(Resources.Load<GameObject>("UI/LargeNonIntractables/level_indicator"), _canvas.transform, true);
        float magicNumber_yOffsetExtra = 20f;

        _levelIndicator.transform.position = middleTop
                                             + new Vector3(0, -GraphicsUtils.GetHeightOf2d(_levelIndicator)/2f, 0)
                                             + new Vector3(0, -magicNumber_yOffsetExtra, 0);
    }

    private void LoadNextTurnButtonsUI()
    {
        _nextTurnButton = GameObject.Instantiate(Resources.Load<GameObject>("UI/LargeClickables/next_turn_card"), _canvas.transform, true);
        _nextTurnWithCorralButton = GameObject.Instantiate(Resources.Load<GameObject>("UI/LargeClickables/next_turn_with_corral"), _canvas.transform, true);
        
        float magicNumber_xOffSetExtra = 10f;
        float magicNumber_yOffsetExtra = 10f;

        _nextTurnButton.transform.localScale = new Vector3(1, 1, 1);
        _nextTurnWithCorralButton.transform.localScale = new Vector3(1, 1, 1);

        Vector3 bottomRight  = new Vector3(Screen.width, 0, 0);
        
        _nextTurnButton.transform.position = bottomRight 
                                             + new Vector3( - GraphicsUtils.GetWidthOf2d(_nextTurnButton), GraphicsUtils.GetHeightOf2d(_nextTurnButton), 0) 
                                             + new Vector3(-magicNumber_xOffSetExtra, magicNumber_yOffsetExtra, 0);
        _nextTurnWithCorralButton.transform.position = _nextTurnButton.transform.position 
                                             + new Vector3(0, GraphicsUtils.GetHeightOf2d(_nextTurnButton), 0) 
                                             + new Vector3(0, magicNumber_yOffsetExtra, 0);
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
        
        float magicNumber_offsetExtra = 15f;
        float yOffset = -GraphicsUtils.GetHeightOf2d(_playerIncome) - magicNumber_offsetExtra;
        
        // place _playerHealth in the top right corner of the screen
        _playerHealth.transform.position = topRight + new Vector3(-GraphicsUtils.GetWidthOf2d(_playerHealth) / 2 - magicNumber_offsetExtra, -GraphicsUtils.GetHeightOf2d(_playerHealth) / 2 - magicNumber_offsetExtra, 0);
        _playerIncome.transform.position = _playerHealth.transform.position + new Vector3(0, yOffset, 0);
        _playerMoney.transform.position = _playerIncome.transform.position + new Vector3(0, yOffset, 0);
    }
    
    private void LoadEnemyStatsUI()
    {
        float magicNumber_offsetExtra = 15f;
        _enemyHealth = GameObject.Instantiate(Resources.Load<GameObject>("UI/Player/EnemyHealth"), _canvas.transform, true);
        _enemyHealth.transform.position = new Vector3(_playerHealth.transform.position.x - GraphicsUtils.GetWidthOf2d(_playerHealth) - magicNumber_offsetExtra, Screen.height - GraphicsUtils.GetHeightOf2d(_enemyHealth) / 2 - magicNumber_offsetExtra, 0);
    }
    
    public void UpdateStatsUI()
    {
        UpdatePlayerStatsUI();
        UpdateEnemyStatsUI();
    }

    private void UpdatePlayerStatsUI()
    {
        // get text from the text mesh pro from _playerHealth
        TextMeshProUGUI textMeshHealth = _playerHealth.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI textMeshIncome = _playerIncome.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI textMeshMoney = _playerMoney.GetComponentInChildren<TextMeshProUGUI>();

        textMeshHealth.text = GlobalVariables.playerGameEngine.player.health.ToString();
        textMeshIncome.text = GlobalVariables.playerGameEngine.player.income.ToString();
        textMeshMoney.text = GlobalVariables.playerGameEngine.player.money.ToString();
    }

    public void UpdateEnemyStatsUI()
    {
        // get text from the text mesh pro from _playerHealth
        TextMeshProUGUI textMeshHealth = _enemyHealth.GetComponentInChildren<TextMeshProUGUI>();
        textMeshHealth.text = GlobalVariables.enemyGameEngine.player.health.ToString();
    }

    private void LoadDeckAndDiscardUI()
    {
        Vector3 bottomLeft = new Vector3(0, 0, 0);

        // Instantiate the deck and card back
        _deckCardBack = GameObject.Instantiate(Resources.Load<GameObject>("UI/LargeClickables/top_deck_card"), _canvas.transform, true);
        _discardCardBack = GameObject.Instantiate(Resources.Load<GameObject>("UI/LargeNonIntractables/discard_card_back"), _canvas.transform, true);
        _usedCardBack = GameObject.Instantiate(Resources.Load<GameObject>("UI/LargeNonIntractables/used_card"), _canvas.transform, true);

        // Just use one scale for both cards, assume they are the same size
        Vector3 scale = _discardCardBack.transform.localScale;
        float magicNumber_xOffSetExtra = 20f;
        float magicNumber_yOffsetExtra = 20f;
        float magicNumber_SpaceBetweenCards = 5f;

        float cardWidth = GraphicsUtils.GetWidthOf2d(_deckCardBack);
        float cardHeight = GraphicsUtils.GetHeightOf2d(_deckCardBack);

        // put the _discardCardBack in the bottom left corner, and put the deck card back above it
        _usedCardBack.transform.position = bottomLeft + new Vector3(cardWidth / 2 + magicNumber_xOffSetExtra,
            cardHeight / 2 + magicNumber_yOffsetExtra, 0);
        _discardCardBack.transform.position = _usedCardBack.transform.position +
                                              new Vector3(0, cardHeight + magicNumber_SpaceBetweenCards, 0);
        _deckCardBack.transform.position = _discardCardBack.transform.position +
                                           new Vector3(0, cardHeight + magicNumber_SpaceBetweenCards, 0);
    }

    public void UpdateLevelIndicatorUI()
    {
        TextMeshProUGUI lvl = _levelIndicator.GetComponentInChildren<TextMeshProUGUI>();
        lvl.text = GlobalVariables.playerGameEngine.currentTurnNumber.ToString();
    }
    
    public void UpdateCreepSendAmountUI()
    {
        TextMeshProUGUI creepAmountImmediate = _nextTurnButton.GetComponentInChildren<TextMeshProUGUI>();
        creepAmountImmediate.text = GlobalVariables.enemyGameEngine.waveManager.creepsInSendImmediate.Count.ToString();
        
        TextMeshProUGUI creepAmountCorral = _nextTurnWithCorralButton.GetComponentInChildren<TextMeshProUGUI>();
        creepAmountCorral.text = GlobalVariables.enemyGameEngine.waveManager.creepsInCorral.Count.ToString();
    }

    private void UpdateDeckTextUI()
    {
        // get the text from the deck card back
        TextMeshProUGUI deckText = _deckCardBack.GetComponentInChildren<TextMeshProUGUI>();
        deckText.text = GlobalVariables.playerGameEngine.cardManager.cardsInDeck.Count.ToString();
        
        // get the text from the discard card back
        TextMeshProUGUI discardText = _discardCardBack.GetComponentInChildren<TextMeshProUGUI>();
        discardText.text = GlobalVariables.playerGameEngine.cardManager.cardsInDiscard.Count.ToString();
        
        // get the text for the used card UI
        TextMeshProUGUI usedText = _usedCardBack.GetComponentInChildren<TextMeshProUGUI>();
        usedText.text = GlobalVariables.playerGameEngine.cardManager.cardsUsedThisTurn.Count.ToString();
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
            GlobalVariablesUtils.ForEachCellInGame(anICell => ToggleHighlightCellAndObjects(anICell, false));
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


    public void OnCardAddedToShop(ICard card)
    {
        
    }
    
    
    // OnCardAddedToHand event
    public void OnCardAddedToHand(ICard card)
    {
    }
    
    private void ResetCardsInShopPosition()
    {
        Vector3 topLeft = new Vector3(0, Screen.height, 0);
        
        List<ICard> cardsInShop = GlobalVariables.playerGameEngine.cardManager.cardsInShop;
        
        for (int i = 0; i < cardsInShop.Count; i++)
        {
            // Get the card game object
            GameObject cardGameObject = cardsInShop[i].GetGameObject();
            
            // make local scale of the cardGameObject smaller
            cardGameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

            float offsetMultiplier = i;
            float magicNumberExtraPaddingY = 20f;
            float magicNumberExtraPaddingX = 10f;
            
            float cardWidth = GraphicsUtils.GetWidthOf2d(cardGameObject);
            float cardHeight = GraphicsUtils.GetHeightOf2d(cardGameObject);
            
            // Put the card game object at the top left of the canvas UI, off set by the width of the card
            cardGameObject.transform.position = topLeft + new Vector3(
                cardWidth / 2 + magicNumberExtraPaddingX + offsetMultiplier * (cardWidth + magicNumberExtraPaddingX),
                -cardHeight / 2 - magicNumberExtraPaddingY,
                0
            );
            
            // Set the card game object to be the child of the canvas
            cardGameObject.transform.SetParent(_canvas.transform, true);
        }
    }
    
    private void LoadShopStaticUI()
    {
        Vector3 topLeft = new Vector3(0, Screen.height, 0);
        
        _rerollButton = GameObject.Instantiate(Resources.Load<GameObject>("UI/LargeClickables/reroll_card"), _canvas.transform, true);
        _levelUpButton = GameObject.Instantiate(Resources.Load<GameObject>("UI/LargeClickables/level_card"), _canvas.transform, true);
        
        // make local scale of the cardGameObject smaller
        _rerollButton.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        _levelUpButton.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        float offsetMultiplier = GlobalVariables.playerGameEngine.config.shopSize;
        
        // Get a random card's width
        GameObject refCard = GlobalVariables.playerGameEngine.cardManager.cardsInShop[0].GetGameObject();
        float cardWidth = GraphicsUtils.GetWidthOf2d(refCard);
        
        float magicNumberExtraPaddingY = 20f;
        float magicNumberExtraPaddingX = 10f;

        // Put the card game object at the top left of the canvas UI, off set by the width of the card
        _levelUpButton.transform.position = topLeft + new Vector3(
            cardWidth / 2 + magicNumberExtraPaddingX + offsetMultiplier * (cardWidth + magicNumberExtraPaddingX),
            -cardWidth / 2 - magicNumberExtraPaddingY/2,
            0
        );

        // Put the card game object at the top left of the canvas UI, off set by the width of the card
        _rerollButton.transform.position = _levelUpButton.transform.position + new Vector3(
            0,
            -GraphicsUtils.GetHeightOf2d(_levelUpButton) - magicNumberExtraPaddingY/2,
            0
        );
        
        
    }

    public void UpdateShopButtonTextUI()
    {
        _levelUpButton.GetComponentInChildren<TextMeshProUGUI>().text = GlobalVariables.playerGameEngine.cardManager.currentCostToLevelUpShop.ToString();
        _rerollButton.GetComponentInChildren<TextMeshProUGUI>().text = GlobalVariables.playerGameEngine.config.rollShopCost.ToString();
    }
    private void ResetCardsInHandPosition()
    {
        // Get bottom center of the UI canvas
        Vector3 bottomCenter = new Vector3(Screen.width / 2f, 0, 0);

        // Get the cards in hand
        GlobalVariables.playerGameEngine.cardManager.cardsInHand.ForEach(aCard =>
        {
            // Get the card game object
            GameObject cardGameObject = aCard.GetGameObject();
            cardGameObject.transform.localScale = new Vector3(1f,1f,1f);

            int totalCards = GlobalVariables.playerGameEngine.cardManager.cardsInHand.Count;
            int cardIndex = GlobalVariables.playerGameEngine.cardManager.cardsInHand.IndexOf(aCard);

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
        if (_selectedCard != null)
        {
            if (_selectedCard.UI_OnCardDeselected() != CardPlayResult.IGNORE)
            {
                return; // This lets the card potentially do something with other cards.
            }
        }

        // Unhighlight all cells and objects.
        GlobalVariables.playerGameEngine.board.GetAllMainBoardCells().ForEach(anICell => ToggleHighlightCellAndObjects(anICell, false));
        
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

    private void OnShopRerolled()
    {
        ResetCardsInShopPosition();
    }

    public void SetupCamera()
    {
        int boardWidth = GlobalVariables.playerGameEngine.config.boardWidth;
        int boardHeight = GlobalVariables.playerGameEngine.config.boardHeight;

        // Calculate the center position of the board
        Vector3 boardCenter = GlobalVariables.playerGameEngine.board.CalculateBoardCenter();

        Camera mainCamera = GlobalVariables.mainCamera;
        
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
        Camera mainCamera = GlobalVariables.mainCamera;
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
