using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.TextCore.LowLevel;

public class WaveManager: MonoBehaviour, IHasIGameEngine
{
    public IGameEngine gameEngine { get; set; }
    public List<ICreep> creepsOnBoard;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private List<GameObject> _refToBoardsPath;

    public void Setup(IGameEngine gameEngine)
    {
        this.gameEngine = gameEngine;
        GlobalVariables.eventManager.creepEventManager.OnCreepKilled += CreepKilled;
        creepsOnBoard = new List<ICreep>();
        
        _refToBoardsPath = gameEngine.board.path;
        _startPos = GraphicsUtils.GetTopOf3d(_refToBoardsPath[0]);
        _endPos = GraphicsUtils.GetTopOf3d(_refToBoardsPath[^1]);
        
        StartCoroutine(SpawnWave(gameEngine.config.waves[0].waveCreeps));
    }

    public void Update()
    {
        MoveCreeps();
    }

    public void SpawnCreep(CreepPreset creepPreset)
    {
        ICreep newCreep = creepPreset.makeCreep();
        newCreep.GetGameObject().transform.position = new Vector3(_startPos.x, _startPos.y, _startPos.z);

        // Set creep rotation face the next cell // TODO this needs to be updated after movement.
        newCreep.GetGameObject().transform.LookAt(_refToBoardsPath[1].transform.position);

        creepsOnBoard.Add(newCreep);
    }
    public IEnumerator SpawnWave(List<CreepPresetWithTime> wavePreset)
    {
        // Copy the list so we don't modify the original >.>
        List<CreepPresetWithTime> wave = new List<CreepPresetWithTime>(wavePreset);
        
        yield return new WaitForEndOfFrame();
        if (wave.Count > 0)
        {
            CreepPresetWithTime creepPresetWithTime = wave[0];
            SpawnCreep(creepPresetWithTime.creepPreset);
            wave.RemoveAt(0);
            yield return new WaitForSeconds(creepPresetWithTime.timeTillNextCreep);
            yield return SpawnWave(wave);
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
        creepsOnBoard.Remove(creep);
    }
}
