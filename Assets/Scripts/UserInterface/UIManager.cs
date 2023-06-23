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
    }

    ~UIManager()
    {
        GlobalVariables.eventManager.cellEventManager.OnCellClick -= OnCellClicked;
        GlobalVariables.eventManager.cardEventManager.OnCardAddedToHand -= OnCardAddedToHand;
        GlobalVariables.eventManager.cardEventManager.OnCardClick -= OnCardClicked;
        GlobalVariables.eventManager.cellEventManager.OnCellMouseEntered -= OnCellEntered;
        GlobalVariables.eventManager.cellEventManager.OnCellMouseExited -= OnCellExited;
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
        GameObject cardGameObject = card.GetGameObject();
        cardGameObject.transform.SetParent(_canvas.transform);
        int x = 100*GlobalVariables.gameEngine.cardManager.cardsInHand.IndexOf(card);
        cardGameObject.transform.localPosition = new Vector3(x, -300, 0); // TODO fix -300?
        
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
}
