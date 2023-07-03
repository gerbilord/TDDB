using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// Create asset menu
[CreateAssetMenu(fileName = "BuildTowerEffect", menuName = "ScriptableObjects/CardEffects/BuildTowerEffect", order = 1)]
public class BuildTowerEffect : ScriptableObject, IPlayEffects
{
    [DoNotSerialize]
    public IGameEngine gameEngine { get; set; }

    [SerializeField]
    private TowerPreset _towerPreset;

    private ITower _phantomTower; // Tower to show on hover when this card is selected.
    
    private ICell _cellToPutTower;
    
    public CardPlayResult Play()
    {
        if (_cellToPutTower.IsBuildable() && gameEngine.board.IsCellInMainBoard(_cellToPutTower))
        {
            gameEngine.board.PlaceTowerOn(_cellToPutTower, _towerPreset.makeTower(gameEngine));
            UI_OnCardDeselected(); // Clean up our UI mess as well.
            return CardPlayResult.SUCCESS;
        }

        return CardPlayResult.IGNORE_BUT_STOP_OTHER_EFFECTS; // If we couldn't build, don't do anything, let the user try to click again.
    }
    
    // Play this card when we selected a cell!
    public CardPlayResult UI_OnCellClicked(ICell cell)
    {
        _cellToPutTower = cell;
        return Play();
    }

    // If the cell is buildable, lets show a phantom tower on hover.
    public CardPlayResult UI_OnCellEntered(ICell cell)
    {
        if (cell.IsBuildable() && gameEngine.board.IsCellInMainBoard(cell))
        {
            // Make a phantom tower on the cell
            _phantomTower = _towerPreset.makeTower(gameEngine);
            _phantomTower.GetGameObject().transform.position = GraphicsUtils.GetTopOf3d(cell.GetGameObject());
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
        // If we are using this card from the UI, it must be the player game engine.
        // If we want the AI to use this card, we will need to inject the AI game engine.
        gameEngine = GlobalVariables.playerGameEngine; 

        List<ICell> allMainBoardCells = gameEngine.board.GetAllMainBoardCells();
        List<ICell> notBuildableCells = allMainBoardCells.Where(anICell => !anICell.IsBuildable()).ToList();
        notBuildableCells.AddRange(gameEngine.board.GetAllCorralCells());
        notBuildableCells.AddRange(gameEngine.board.GetAllImmediateSendCells());
 
        // Make dark red color
        Color darkRed = new Color(0.5f, 0, 0, 0.7f);
        notBuildableCells.ForEach(anICell => GlobalVariables.uiManager.ToggleHighlightCellAndObjects(anICell, true, darkRed, .7f)); // TODO: Move this to a util?
 
        return CardPlayResult.IGNORE;
    }

    // When we deselect the card, unhighlight all cells, and remove the phantom tower. 
    public CardPlayResult UI_OnCardDeselected()
    {
        GlobalVariablesUtils.ForEachCellInGame(anICell => GlobalVariables.uiManager.ToggleHighlightCellAndObjects(anICell, false));
        UI_OnCellExited(null); // Clean up our phantom tower as well.

        return CardPlayResult.IGNORE;
    }

    public void InjectPlayData(ICell cellToPutTower, TowerPreset whatTowerToPlace)
    {
        _cellToPutTower = cellToPutTower;
        _towerPreset = whatTowerToPlace;
    }
}