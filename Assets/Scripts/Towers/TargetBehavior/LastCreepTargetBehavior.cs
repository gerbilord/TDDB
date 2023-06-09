﻿
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LastCreepTargetBehavior: ITowerTargetBehavior
{
    public IGameEngine gameEngine { get; set; }
    
    // Constructor
    public LastCreepTargetBehavior(IGameEngine gameEngine)
    {
        this.gameEngine = gameEngine;
    }
    public ICreep getTarget(Vector3 pos, float range)
    {
        List<ICreep> lastCreeps = gameEngine.waveManager.creepsOnBoard.OrderBy(creep => creep.currentPathIndex).ToList();
        
        // Filter out creeps that are out of range
        lastCreeps = lastCreeps.Where(creep => Vector3.Distance(pos, creep.GetGameObject().transform.position) <= range).ToList();
        
        if(lastCreeps.Count == 0)
            return null;
        return lastCreeps[^1];
        
    }
}
