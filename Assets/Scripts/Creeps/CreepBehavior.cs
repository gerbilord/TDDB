using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepBehavior : MonoBehaviour, ICreep
{

    public Dictionary<StatType, float> stats { get; set; }
    public int currentPathIndex { get; set; }

    private Animator _animator;

    public void Awake()
    {
        // Get animator
        _animator = GetComponent<Animator>();
    }
    
    // If the creep was clicked
    public void OnMouseDown()
    {
        // Change animator trigger "Clicked" to true. Turns back to false automatically after animation is done.
        _animator.SetTrigger("Clicked");
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
