using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// Create asset menu
[CreateAssetMenu(fileName = "AddCreepToCorralEffect", menuName = "ScriptableObjects/CardEffects/AddCreepToCorralEffect", order = 1)]
public class AddCreepToCorralEffect : ScriptableObject, IPlayEffects
{
    [DoNotSerialize]
    public IGameEngine gameEngine { get; set; }

    [SerializeField]
    private CreepPreset _creepPreset;

    private ICreep _phantomCreep; // Creep to show on hover when this card is selected.
    private ICell _cellToPutCreep;
    
    private CardWhereToSend _whereToSend;
    
    
    public CardPlayResult Play()
    {
        if (gameEngine.board.IsCellInImmediateSend(_cellToPutCreep) || gameEngine.board.IsCellInCorral(_cellToPutCreep))
        {
            if (gameEngine.board.IsCellInImmediateSend(_cellToPutCreep))
            {
                gameEngine.SendCreepToEnemyImmediateSend(_creepPreset);
                _whereToSend = CardWhereToSend.DEFAULT;
            }
            else
            {
                gameEngine.SendCreepToEnemyCorral(_creepPreset);
                _whereToSend = CardWhereToSend.CORRAL;
            }
            UI_OnCardDeselected(); // Clean up our UI mess as well.
            return CardPlayResult.SUCCESS;
        }
        
        return CardPlayResult.IGNORE_BUT_STOP_OTHER_EFFECTS; // If we couldn't put a creep, don't do anything, let the user try to click again.
    }
    
    public void InjectPlayData(ICell cellToPutCreep, CreepPreset creepPreset)
    {
        _cellToPutCreep = cellToPutCreep;
        _creepPreset = creepPreset;
    }

    public CardWhereToSend GetWhereToSendThisCard()
    {
        return _whereToSend;
    }
    
    // Play this card when we selected a cell!
    public CardPlayResult UI_OnCellClicked(ICell cell)
    {
        _cellToPutCreep = cell;
        return Play();
    }

    // If the cell can take a creep, lets show a phantom creep on hover.
    public CardPlayResult UI_OnCellEntered(ICell cell)
    {
        // Make a phantom creep!

        return CardPlayResult.IGNORE;
    }
    
    // Clean up our phantom tower when we exit the cell.
    public CardPlayResult UI_OnCellExited(ICell cell)
    {
        // Destroy the phantom creep. 

        return CardPlayResult.IGNORE;
    }

    // When we select the card, mark all unbuildable cells in red.
    public CardPlayResult UI_OnCardSelected()
    {
        // If we are using this card from the UI, it must be the player game engine.
        // If we want the AI to use this card, we will need to inject the AI game engine.
        gameEngine = GlobalVariables.playerGameEngine; 

        List<ICell> allMainBoardCells = gameEngine.board.GetAllMainBoardCells();
        List<ICell> notBuildableCells = allMainBoardCells;
        
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
}