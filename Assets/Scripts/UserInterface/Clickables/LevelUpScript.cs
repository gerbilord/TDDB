using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpScript : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (GlobalVariables.playerGameEngine.cardManager.currentShopLevel + 1 <
            GlobalVariables.playerGameEngine.config.shopLevelCardPresets.Count)
        {
            GlobalVariables.playerGameEngine.player.SpendMoney(GlobalVariables.playerGameEngine.cardManager.currentCostToLevelUpShop);
            GlobalVariables.playerGameEngine.cardManager.LevelUpShop();
        }
    }
}