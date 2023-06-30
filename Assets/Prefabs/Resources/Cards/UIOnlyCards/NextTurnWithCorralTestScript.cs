using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NextTurnWithCorralTestScript : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (
            GlobalVariables.playerGameEngine.waveManager.IsWaveOver() &&
            GlobalVariables.enemyGameEngine.waveManager.IsWaveOver()
        )
        {
            GlobalVariables.enemyGameEngine.waveManager.SendCreepsInCorral();
            GlobalVariables.playerGameEngine.cardManager.MoveCorralCardsToDiscard();

            GlobalVariables.playerGameEngine.FinishCardTurn_StartWave();
            GlobalVariables.enemyGameEngine.FinishCardTurn_StartWave();
        }
    }
}
