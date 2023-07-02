using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NextTurnClickScript : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (
            GlobalVariables.playerGameEngine.waveManager.IsWaveOver() &&
            GlobalVariables.enemyGameEngine.waveManager.IsWaveOver()
            )
        {
            GlobalVariables.playerGameEngine.FinishCardTurn_StartWave();
            GlobalVariables.enemyGameEngine.FinishCardTurn_StartWave();
        }
        
    }
}
