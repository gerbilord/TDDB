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
    }