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

    public void SpendMoney(int amount)
    {
        money -= amount;
        GlobalVariables.uiManager.UpdateStatsUI();
    }
    
    public void AddMoney(int amount)
    {
        money += amount;
        GlobalVariables.uiManager.UpdateStatsUI();
    }
    
    
    public void AddIncomeToMoney()
    {
        AddMoney(income);
    }

    private void CreepLeaked(ICreep creep, IGameEngine gameEngine)
    {
        if(gameEngine == this.gameEngine)
        {
            health -= 1; // TODO make creep effect this.
            GlobalVariables.uiManager.UpdateStatsUI();
        }
    }

    // Destructor
    ~Player()
    {
        GlobalVariables.eventManager.creepEventManager.OnCreepLeaked -= CreepLeaked;
    }
}
