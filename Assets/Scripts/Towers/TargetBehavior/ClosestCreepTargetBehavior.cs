
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClosestCreepTargetBehavior: ITowerTargetBehavior
{
    public IGameEngine gameEngine { get; set; }
    
    // Constructor
    public ClosestCreepTargetBehavior(IGameEngine gameEngine)
    {
        this.gameEngine = gameEngine;
    }

    public ICreep getTarget(Vector3 pos, float range)
    {
        // Order creeps by distance from tower
        List<ICreep>closestCreeps = gameEngine.waveManager.creepsOnBoard.OrderBy(creep => Vector3.Distance(pos, creep.GetGameObject().transform.position)).ToList();
        
        // Filter out creeps that are out of range
        closestCreeps = closestCreeps.Where(creep => Vector3.Distance(pos, creep.GetGameObject().transform.position) <= range).ToList();
        
        if(closestCreeps.Count == 0)
            return null;
        return closestCreeps[0];
        
    }
}
