using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerClickHandler, ICard
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GlobalVariables.eventManager.cardEventManager.CardClicked(this);
    }
    
    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}
