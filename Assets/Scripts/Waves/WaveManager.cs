using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.TextCore.LowLevel;

public class WaveManager: MonoBehaviour, IHasIGameEngine
{
    public IGameEngine gameEngine { get; set; }
    public List<ICreep> creepsOnBoard;
    public List<CreepPresetWithTime> creepsYetToSpawnInWave;
    public List<CreepPreset> creepsInCorral;          // THIS IS TECHNICALLY THE ENEMY CORRAL
    public List<CreepPreset> creepsInSendImmediate;   // THIS IS TECHNICALLY THE ENEMY CREEPS TO SEND IMMEDIATELY
    private Vector3 _startPos;
    private Vector3 _endPos;
    private List<GameObject> _refToBoardsPath;
    private GameObject _creepHierarchyParent;

    public void Setup(IGameEngine gameEngine)
    {
        this.gameEngine = gameEngine;
        GlobalVariables.eventManager.creepEventManager.OnCreepKilled += CreepKilled;
        creepsOnBoard = new List<ICreep>();
        creepsInCorral = new List<CreepPreset>();
        creepsYetToSpawnInWave = new List<CreepPresetWithTime>();
        creepsInSendImmediate = new List<CreepPreset>();
        
        // create _creepHierarchyParent
        _creepHierarchyParent = new GameObject("Creeps");

        _refToBoardsPath = gameEngine.board.path;
        _startPos = GraphicsUtils.GetTopOf3d(_refToBoardsPath[0]);
        _endPos = GraphicsUtils.GetTopOf3d(_refToBoardsPath[^1]);
    }

    public void AddCreepToCorral(CreepPreset creepPreset)
    {
        creepsInCorral.Add(creepPreset);
        GlobalVariables.uiManager.UpdateCreepSendAmountUI();
    }

    public void AddCreepToSendImmediate(CreepPreset creepPreset)
    {
        creepsInSendImmediate.Add(creepPreset);
        GlobalVariables.uiManager.UpdateCreepSendAmountUI();
    }
    
    public void SendCreepsInCorral()
    {
        creepsInSendImmediate.AddRange(creepsInCorral);
        creepsInCorral.Clear();
        GlobalVariables.uiManager.UpdateCreepSendAmountUI();
    }

    public void SpawnWave(int turnNumber)
    {
        // Copy the list so we don't modify the original >.>
        List<CreepPresetWithTime> creepsToSpawn = new List<CreepPresetWithTime>();

        creepsToSpawn.AddRange(creepsInSendImmediate.Select(preset => new CreepPresetWithTime(preset, .2f)).ToList());
        creepsInSendImmediate.Clear();
        GlobalVariables.uiManager.UpdateCreepSendAmountUI();

        creepsToSpawn.AddRange(gameEngine.config.waves[turnNumber].waveCreeps);

        creepsYetToSpawnInWave = creepsToSpawn;

        StartCoroutine(SpawnCurrentWave());
    }

    public void Update()
    {
        MoveCreeps();
    }

    private void SpawnCreep(CreepPreset creepPreset)
    {
        ICreep newCreep = creepPreset.makeCreep();
        
        newCreep.GetGameObject().transform.SetParent(_creepHierarchyParent.transform);
        
        newCreep.GetGameObject().transform.position = new Vector3(_startPos.x, _startPos.y, _startPos.z);

        // Set creep rotation face the next cell // TODO this needs to be updated after movement.
        newCreep.GetGameObject().transform.LookAt(_refToBoardsPath[1].transform.position);

        creepsOnBoard.Add(newCreep);
    }
    public IEnumerator SpawnCurrentWave()
    {
        yield return new WaitForEndOfFrame();
        if (creepsYetToSpawnInWave.Count > 0)
        {
            CreepPresetWithTime creepPresetWithTime = creepsYetToSpawnInWave[0];
            SpawnCreep(creepPresetWithTime.creepPreset);
            creepsYetToSpawnInWave.RemoveAt(0);
            yield return new WaitForSeconds(creepPresetWithTime.timeTillNextCreep);
            yield return SpawnCurrentWave();
        }
        else { yield return null; }
    }
    private void MoveCreeps()
    {
        //move each creepPreset on the board down the path, then increment their current path index
        for (int i = 0; i < creepsOnBoard.Count; i++)
        {
            ICreep creep = creepsOnBoard[i];
            Vector3 des = GraphicsUtils.GetTopOf3d(_refToBoardsPath[creep.currentPathIndex + 1]);
            Vector3 start = creep.GetGameObject().transform.position;

            if (start == des){
                if (creep.currentPathIndex < _refToBoardsPath.Count-2)
                {
                    creep.currentPathIndex += 1;
                    creep.GetGameObject().transform.LookAt(_refToBoardsPath[creep.currentPathIndex + 1].transform.position);
                }
                else
                {
                    OnLeak(creep);
                    break;
                }
            }
            Vector3 end = GraphicsUtils.GetTopOf3d(_refToBoardsPath[creep.currentPathIndex + 1]);
            float moveSpeed = creep.stats[StatType.moveSpeed] * Time.deltaTime;
            creep.GetGameObject().transform.position = Vector3.MoveTowards(start, end, moveSpeed);
        }
    }

    public void OnLeak(ICreep creep)
    {
        creep.killCreep();
        GlobalVariables.eventManager.creepEventManager.CreepLeaked(creep, gameEngine);
    }
    private void CreepKilled(ICreep creep)
    {
        if (creepsOnBoard.Contains(creep))
        {
            creepsOnBoard.Remove(creep);
            GameObject.Destroy(creep.GetGameObject());
            EndWaveIfNoCreepsLeft();
        }
    }

    private void EndWaveIfNoCreepsLeft()
    {
        if (IsWaveOver())
        {
            gameEngine.OnWaveEnd_StartCardTurn();
        }
    }

    public bool IsWaveOver()
    {
        return creepsYetToSpawnInWave.Count == 0 && creepsOnBoard.Count == 0;
    }
}
