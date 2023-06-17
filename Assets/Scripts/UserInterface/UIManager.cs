using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager
{

    private Canvas _canvas;
    private GameObject _selectedCell;
    private GameObject _selectedCard;
    
    public UIManager()
    {
        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        
        GlobalVariables.eventManager.cellEventManager.OnCellClick += OnCellClicked;
        GlobalVariables.eventManager.cardEventManager.OnCardAddedToHand += OnCardAddedToHand;
        GlobalVariables.eventManager.cardEventManager.OnCardClick += OnCardClicked;
    }

    ~UIManager()
    {
        GlobalVariables.eventManager.cellEventManager.OnCellClick -= OnCellClicked;
        GlobalVariables.eventManager.cardEventManager.OnCardAddedToHand -= OnCardAddedToHand;
        GlobalVariables.eventManager.cardEventManager.OnCardClick -= OnCardClicked;
    }
    
    // OnCellClicked event
    public void OnCellClicked(ICell cell)
    {
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
            GlobalVariables.gameEngine.board.PlaceTowerOn(cell);
            ToggleHighlight(_selectedCard, false);
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
        cardGameObject.transform.localPosition = new Vector3(0, -300, 0); // TODO fix -300?
        
    }

    public void OnCardClicked(ICard cardClicked)
    {
        GameObject oldCard = _selectedCard;
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

        _selectedCard = newCard;
        newCard.GetComponent<IHighlighter>().ToggleHighlight(true);


        List<ICell> allCells = GlobalVariables.gameEngine.board.GetAllCells();
        
        List<ICell> buildableCells = allCells.Where(anICell => anICell.IsBuildable()).ToList();
        List<ICell> notBuildableCells = allCells.Where(anICell => !anICell.IsBuildable()).ToList();
        
        
        // Make dark green color
        Color darkGreen = new Color(0, .5f, 0);
        
        buildableCells.ForEach(anICell => ToggleHighlightCellAndObjects(anICell, true, darkGreen, .3f));
        notBuildableCells.ForEach(anICell => ToggleHighlightCellAndObjects(anICell, true, Color.red, .3f));
    }
}
