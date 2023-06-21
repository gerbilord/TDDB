using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager: MonoBehaviour
{
    private GameEngine _gameEngine;
    public List<ICreep> creepsOnBoard;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private List<GameObject> _refToBoardsPath;

    public void Start()
    {
        creepsOnBoard = new List<ICreep>();
        
        _refToBoardsPath = GlobalVariables.gameEngine.board.path;
        _startPos = _refToBoardsPath[0].transform.position;
        _endPos = _refToBoardsPath[^1].transform.position;
        
        StartCoroutine(SpawnWave(GlobalVariables.config.waves[0].waveCreeps, 2));
    }

    public void Update()
    {
        MoveCreeps();
    }

    public void SpawnCreep(CreepPreset creepPreset)
    {
        GameObject creepObject = Instantiate(creepPreset.prefab, new Vector3(_startPos.x, 0.5f, _startPos.z), Quaternion.identity);
        creepObject.GetComponent<ICreep>().creepMoveSpeed = creepPreset.moveSpeed;
        creepsOnBoard.Add(creepObject.GetComponent<ICreep>());
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
            Vector3 des = _refToBoardsPath[creep.currentPathIndex + 1].transform.position;
            Vector3 start = creep.GetGameObject().transform.position;
            //this value changes based off of the model size of the creepPreset
            des.y += .5f;
            if (start == des){
                if (creep.currentPathIndex < _refToBoardsPath.Count-2)
                {
                    creep.currentPathIndex += 1;
                }
                else
                {
                    OnLeak(creep);
                    break;
                }
            }
            Vector3 end = _refToBoardsPath[creep.currentPathIndex + 1].transform.position;
            end.y += .5f;
            float moveSpeed = creep.creepMoveSpeed * Time.deltaTime;
            creep.GetGameObject().transform.position = Vector3.MoveTowards(start, end, moveSpeed);
        }
    }
    public void OnLeak(ICreep creep)
    {
        KillCreep(creep);
    }
    public void KillCreep(ICreep creep)
    {
        creepsOnBoard.Remove(creep);
        Destroy(creep.GetGameObject());
    }
}
