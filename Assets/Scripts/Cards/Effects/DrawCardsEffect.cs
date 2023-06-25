using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Create asset menu
[CreateAssetMenu(fileName = "DrawCardsEffect", menuName = "ScriptableObjects/CardEffects/DrawCardsEffect", order = 1)]
public class DrawCardsEffect : ScriptableObject, IPlayEffects
{
    [SerializeField]
    private int numCardsToDraw;
    
    public CardPlayResult Play(ICell cell)
    {
        GlobalVariables.gameEngine.cardManager.DrawCards(numCardsToDraw);

        return CardPlayResult.SUCCESS;
    }
    
    // UI On Clicked
    public CardPlayResult UI_OnCellClicked(ICell cell)
    {
        return Play(cell);
    }

}
