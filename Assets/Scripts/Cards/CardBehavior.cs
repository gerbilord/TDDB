using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBehavior : MonoBehaviour, IPointerClickHandler, ICard
{
    public ITower tower { get; set; }
    public TowerPreset towerPreset { get; set; }
    public void OnPointerClick (PointerEventData eventData)
    {
        GlobalVariables.eventManager.cardEventManager.CardClicked(this);
    }
    
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void Play(ICell cell)
    {
        if (cell.IsBuildable())
            GlobalVariables.gameEngine.board.PlaceTowerOn(cell, towerPreset.makeTower().GetComponent<ITower>());
    }
}
