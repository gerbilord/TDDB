using UnityEngine;
using UnityEngine.EventSystems;

    public class CreepEventManager
    {
        //Create event for when a creep is killed
        public delegate void CreepKill(ICreep creep);
        public event CreepKill OnCreepKilled;
        public void CreepKilled(ICreep creep)
        {
            OnCreepKilled?.Invoke(creep);
        }
        
        // Create event for when a creep is leaked
        public delegate void CreepLeak(ICreep creep, IGameEngine gameEngine);
        public event CreepLeak OnCreepLeaked;
        public void CreepLeaked(ICreep creep, IGameEngine gameEngine)
        {
            OnCreepLeaked?.Invoke(creep, gameEngine);
        }
    }