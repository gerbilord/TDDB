using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// Create asset menu
[CreateAssetMenu(fileName = "DrawCardsEffect", menuName = "ScriptableObjects/CardEffects/DrawCardsEffect", order = 1)]
public class DrawCardsEffect : ScriptableObject, IPlayEffects
{
    [DoNotSerialize]
    public IGameEngine gameEngine { get; set; }
    
    [SerializeField]
    private int numCardsToDraw;
    
    public CardPlayResult Play()
    {
        gameEngine.cardManager.DrawCards(numCardsToDraw);

        return CardPlayResult.SUCCESS;
    }
    
    public CardPlayResult UI_OnCardSelected()
    {
        // If we are using this card from the UI, it must be the player game engine.
        // If we want the AI to use this card, we will need to inject the AI game engine.
        gameEngine = GlobalVariables.playerGameEngine;

        return CardPlayResult.IGNORE;
    }

    // UI On Clicked
    public CardPlayResult UI_OnCellClicked(ICell cell)
    {
        return Play();
    }
}
