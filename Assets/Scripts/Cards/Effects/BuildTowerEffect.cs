using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Create asset menu
[CreateAssetMenu(fileName = "BuildTowerEffect", menuName = "ScriptableObjects/CardEffects/BuildTowerEffect", order = 1)]
public class BuildTowerEffect : ScriptableObject, IPlayEffects
{
    [SerializeField]
    private TowerPreset _towerPreset;

    private ITower _phantomTower; // Tower to show on hover when this card is selected.
    
    public CardPlayResult Play(ICell cell)
    {
        if (cell.IsBuildable())
        {
            GlobalVariables.gameEngine.board.PlaceTowerOn(cell, _towerPreset.makeTower());
            UI_OnCardDeselected(); // Clean up our UI mess as well.
            return CardPlayResult.SUCCESS;
        }

        return CardPlayResult.IGNORE; // If we couldn't build, don't do anything, let the user try to click again.
    }
    
    // Play this card when we selected a cell!
    public CardPlayResult UI_OnCellClicked(ICell cell)
    {
        return Play(cell);
    }

    // If the cell is buildable, lets show a phantom tower on hover.
    public CardPlayResult UI_OnCellEntered(ICell cell)
    {
        if (cell.IsBuildable())
        {
            // Make a phantom tower on the cell
            _phantomTower = _towerPreset.makeTower();
            _phantomTower.GetGameObject().transform.position = GraphicsUtils.GetTopOf(cell.GetGameObject());
            ((MonoBehaviour)_phantomTower).enabled = false;

            // Make the phantom tower transparent
            _phantomTower.GetGameObject().GetComponent<IOpacityChanger>().ToggleOpacity(true);
        
            // Set _phantomTower isPlacing animation bool to true
            _phantomTower.GetGameObject().GetComponent<Animator>().SetBool("isPlacing", true);

            // In the viewer name it phantom tower
            _phantomTower.GetGameObject().name = "Phantom Tower";

        }

        return CardPlayResult.IGNORE;
    }
    
    // Clean up our phantom tower when we exit the cell.
    public CardPlayResult UI_OnCellExited(ICell cell)
    {
        // Destroy the phantom tower
        if (_phantomTower != null)
        {
            GameObject.Destroy(_phantomTower.GetGameObject());
            _phantomTower = null;
        }

        return CardPlayResult.IGNORE;
    }

    // When we select the card, mark all unbuildable cells in red.
    public CardPlayResult UI_OnCardSelected()
    {
        List<ICell> allCells = GlobalVariables.gameEngine.board.GetAllCells();
        List<ICell> notBuildableCells = allCells.Where(anICell => !anICell.IsBuildable()).ToList();
 
        // Make dark red color
        Color darkRed = new Color(0.5f, 0, 0, 0.7f);
        notBuildableCells.ForEach(anICell => GlobalVariables.uiManager.ToggleHighlightCellAndObjects(anICell, true, darkRed, .7f)); // TODO: Move this to a util?
 
        return CardPlayResult.IGNORE;
    }

    // When we deselect the card, unhighlight all cells, and remove the phantom tower. 
    public CardPlayResult UI_OnCardDeselected()
    {
        GlobalVariables.gameEngine.board.GetAllCells().ForEach(anICell => GlobalVariables.uiManager.ToggleHighlightCellAndObjects(anICell, false));
        UI_OnCellExited(null); // Clean up our phantom tower as well.

        return CardPlayResult.IGNORE;
    }
    
}
