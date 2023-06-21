using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager: MonoBehaviour
{
    private GameEngine _gameEngine;
    public List<GameObject> creepsOnBoard;
    private Vector3 _startPos;
    private Vector3 _endPos;
    private List<GameObject> _refToBoardsPath;

    public void Start()
    {
        creepsOnBoard = new List<GameObject>();
        
        _refToBoardsPath = GlobalVariables.gameEngine.board.path;
        _startPos = GraphicsUtils.GetTopOf(_refToBoardsPath[0]);
        _endPos = GraphicsUtils.GetTopOf(_refToBoardsPath[^1]);
        
        StartCoroutine(SpawnWave(GlobalVariables.config.waves[0].waveCreeps, 2));
    }

    public void SpawnCreep(Creep creep)
    {

        GameObject creepObject = Instantiate(creep.prefab, new Vector3(_startPos.x, _startPos.y, _startPos.z), Quaternion.identity);

        // Set creep rotation face the next cell // TODO this needs to be updated after movement.
        creepObject.transform.LookAt(_refToBoardsPath[1].transform.position);

        creepsOnBoard.Add(creepObject);
    }
    public IEnumerator SpawnWave(List<Creep> wavePreset, float delay)
    {
        // Copy the list so we don't modify the original >.>
        List<Creep> wave = new List<Creep>(wavePreset);
        
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
}
