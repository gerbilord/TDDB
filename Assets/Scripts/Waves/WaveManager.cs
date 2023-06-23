using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore;
using UnityEngine.TextCore.LowLevel;

public class WaveManager: MonoBehaviour
{
    private GameEngine _gameEngine;
    public List<ICreep> creepsOnBoard;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private List<GameObject> _refToBoardsPath;

    public void Start()
    {
        GlobalVariables.eventManager.creepEventManager.OnCreepKilled += CreepKilled;
        creepsOnBoard = new List<ICreep>();
        
        _refToBoardsPath = GlobalVariables.gameEngine.board.path;
        _startPos = GraphicsUtils.GetTopOf(_refToBoardsPath[0]);
        _endPos = GraphicsUtils.GetTopOf(_refToBoardsPath[^1]);
        
        StartCoroutine(SpawnWave(GlobalVariables.config.waves[0].waveCreeps, 2));
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
    public IEnumerator SpawnWave(List<CreepPreset> wavePreset, float delay)
    {
        // Copy the list so we don't modify the original >.>
        List<CreepPreset> wave = new List<CreepPreset>(wavePreset);
        
        yield return new WaitForEndOfFrame();
        if (wave.Count > 0)
        {
            SpawnCreep(wave[0]);
            wave.RemoveAt(0);
            yield return new WaitForSeconds(delay);
            yield return SpawnWave(wave, delay);
        }
        else { yield return null; }
    }
    private void MoveCreeps()
    {
        //move each creepPreset on the board down the path, then increment their current path index
        for (int i = 0; i < creepsOnBoard.Count; i++)
        {
            ICreep creep = creepsOnBoard[i];
            Vector3 des = GraphicsUtils.GetTopOf(_refToBoardsPath[creep.currentPathIndex + 1]);
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
            Vector3 end = GraphicsUtils.GetTopOf(_refToBoardsPath[creep.currentPathIndex + 1]);
            float moveSpeed = creep.stats[StatType.moveSpeed] * Time.deltaTime;
            creep.GetGameObject().transform.position = Vector3.MoveTowards(start, end, moveSpeed);
        }
    }

    public void OnLeak(ICreep creep)
    {
        creep.killCreep();
    }
    private void CreepKilled(ICreep creep)
    {
        creepsOnBoard.Remove(creep);
    }
}
