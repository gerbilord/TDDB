
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LastCreepTargetBehavior: ITowerTargetBehavior
{
    public ICreep getTarget(Vector3 pos, float range)
    {
        //TODO: Implement range of towers
        List<ICreep>lastCreeps = GlobalVariables.gameEngine.waveManager.creepsOnBoard.OrderBy(creep => creep.currentPathIndex).ToList();
        if(lastCreeps.Count == 0)
            return null;
        return lastCreeps[^1];
        
    }
}
