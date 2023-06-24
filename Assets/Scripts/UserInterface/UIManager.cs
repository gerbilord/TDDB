using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager
{

    private Canvas _canvas;
    private GameObject _selectedCell;
    private ICard _selectedCard;
    private ITower _phantomTower;
    
    public UIManager()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        
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

    public void OnCellEntered(ICell cell)
    {
        if(_phantomTower != null) // Destroy previous phantoms, we only want one at a time
        {
            GameObject.Destroy(_phantomTower.GetGameObject());
            _phantomTower = null; // Is this needed? Or will it auto become null?
        }

        if (_selectedCard != null && cell.IsBuildable())
        {
            // Make a phantom tower on the cell
            _phantomTower = _selectedCard.towerPreset.makeTower();
            _phantomTower.GetGameObject().transform.position = GraphicsUtils.GetTopOf(cell.GetGameObject());
            ((MonoBehaviour)_phantomTower).enabled = false;

            // Make the phantom tower transparent
            _phantomTower.GetGameObject().GetComponent<IOpacityChanger>().ToggleOpacity(true);
            
            // Set _phantomTower isPlacing animation bool to true
            _phantomTower.GetGameObject().GetComponent<Animator>().SetBool("isPlacing", true);

            // In the viewer name it phantom tower
            _phantomTower.GetGameObject().name = "Phantom Tower";
        }
    }

    public void OnCellExited(ICell cell)
    {
        if(_phantomTower != null)
        {
            GameObject.Destroy(_phantomTower.GetGameObject());
            _phantomTower = null;
        }
    }
    
    public void OnCellClicked(ICell cell)
    {
        if(_phantomTower != null)
        {
            GameObject.Destroy(_phantomTower.GetGameObject());
            _phantomTower = null;
        }

        GameObject oldCell = _selectedCell;
        _selectedCell = cell.GetGameObject();
        
        GlobalVariables.gameEngine.board.GetAllCells().ForEach(anICell => ToggleHighlightCellAndObjects(anICell, false));
        
        /* Currently not needed with above statement
        if(oldCell != null)
        {
            ToggleHighlightCellAndObjects(oldCell.GetComponent<ICell>(), false);
        }
        */

        if (_selectedCard != null)
        {
            _selectedCard.Play(cell);
            ToggleHighlight(_selectedCard.GetGameObject(), false);
            _selectedCard = null;
            return;
        }
        
        _selectedCard = null;
        ToggleHighlightCellAndObjects(cell, true);
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
        GameObject oldCard = null;
        if (_selectedCard != null)
        {
            oldCard = _selectedCard.GetGameObject();
        }

        GameObject newCard = cardClicked.GetGameObject();

        if (oldCard != null)
        {
            oldCard.GetComponent<IHighlighter>().ToggleHighlight(false);
        }
        if(oldCard == newCard)
        {
            GlobalVariables.gameEngine.board.GetAllCells().ForEach(anICell => ToggleHighlightCellAndObjects(anICell, false));
            _selectedCard = null;
            return;
        }

        _selectedCard = newCard.GetComponent<ICard>();
        newCard.GetComponent<IHighlighter>().ToggleHighlight(true);


        List<ICell> allCells = GlobalVariables.gameEngine.board.GetAllCells();
        List<ICell> notBuildableCells = allCells.Where(anICell => !anICell.IsBuildable()).ToList();
        
        // Make dark red color
        Color darkRed = new Color(0.5f, 0, 0, 0.7f);
        notBuildableCells.ForEach(anICell => ToggleHighlightCellAndObjects(anICell, true, darkRed, .7f));
    }

    public void OnCardPlayed(ICard cardPlayed)
    {
    }

    public void OnHandSizeChanged()
    {
        ResetCardsInHandPosition();
    }
}
