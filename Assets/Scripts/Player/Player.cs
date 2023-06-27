using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public int income;
    public int money;
    public int health;
    
    // Constructor
    public Player()
    {
        income = GlobalVariables.config.startingIncome;
        money = GlobalVariables.config.startingMoney;
        health = GlobalVariables.config.startingHealth;
        
        GlobalVariables.eventManager.creepEventManager.OnCreepLeaked += CreepLeaked;
    }

    private void CreepLeaked(ICreep creep)
    {
        health -= 1; // TODO make creep effect this.
        GlobalVariables.uiManager.UpdatePlayerStatsUI();
    }

    // Destructor
    ~Player()
    {
        GlobalVariables.eventManager.creepEventManager.OnCreepLeaked -= CreepLeaked;
    }
}
