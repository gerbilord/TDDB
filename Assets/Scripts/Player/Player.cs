using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : IHasIGameEngine
{
    public IGameEngine gameEngine { get; set; }

    public int income;
    public int money;
    public int health;
    
    // Constructor
    public Player(IGameEngine gameEngine)
    {
        this.gameEngine = gameEngine;

        income = gameEngine.config.startingIncome;
        money = gameEngine.config.startingMoney;
        health = gameEngine.config.startingHealth;
        
        GlobalVariables.eventManager.creepEventManager.OnCreepLeaked += CreepLeaked;
    }

    private void CreepLeaked(ICreep creep, IGameEngine gameEngine)
    {
        if(gameEngine == this.gameEngine)
        {
            health -= 1; // TODO make creep effect this.
            GlobalVariables.uiManager.UpdatePlayerStatsUI();
        }
    }

    // Destructor
    ~Player()
    {
        GlobalVariables.eventManager.creepEventManager.OnCreepLeaked -= CreepLeaked;
    }
}
