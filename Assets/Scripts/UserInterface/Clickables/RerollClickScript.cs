using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RerollClickScript : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GlobalVariables.playerGameEngine.player.money >= GlobalVariables.playerGameEngine.config.rollShopCost || GlobalVariables.playerGameEngine.config.rollShopCost == 0) 
        {
            GlobalVariables.playerGameEngine.player.SpendMoney(GlobalVariables.playerGameEngine.config.rollShopCost);
            GlobalVariables.uiManager.UpdateStatsUI();
            GlobalVariables.playerGameEngine.cardManager.RerollShop();
        }
    }
}