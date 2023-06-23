using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make TowerTargetBehaviorEnum
public enum TowerTargetBehaviorEnum
{
    // FirstCreep,
    LastCreep,
    // StrongestCreep,
    // WeakestCreep,
    // RandomCreep
}
[CreateAssetMenu(fileName = "TowerTargetingPreset", menuName = "ScriptableObjects/TowerTargetingPreset", order = 1)]
public class TowerTargetingPreset : ScriptableObject
{
    public TowerTargetBehaviorEnum towerTargetingBehaviorEnum;
    
    public ITowerTargetBehavior MakeTowerTargetingBehavior()
    {
        // Switch on towerTargetingBehaviorEnum
        switch (towerTargetingBehaviorEnum)
        {
            // case TowerTargetBehaviorEnum.FirstCreep:
            //     return new FirstCreepTargetBehavior();
            case TowerTargetBehaviorEnum.LastCreep:
                return new LastCreepTargetBehavior();
            // case TowerTargetBehaviorEnum.StrongestCreep:
            //     return new StrongestCreepTargetBehavior();
            // case TowerTargetBehaviorEnum.WeakestCreep:
            //     return new WeakestCreepTargetBehavior();
            // case TowerTargetBehaviorEnum.RandomCreep:
            //     return new RandomCreepTargetBehavior();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
