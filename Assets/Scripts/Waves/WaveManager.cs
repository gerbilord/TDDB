using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    GameEngine gameEngine;
    Vector3 startPos;
    Vector3 endPos;
    List<GameObject> creepsOnBoard;
    List<GameObject> path;

    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setPath()
    {
        gameEngine = GlobalVariables.gameEngine;
        path = gameEngine.board.path;
        print(path);
        print(path[0]);
        startPos = path[0].transform.position;
        endPos = path[path.Count - 1].transform.position;
    }
    public void spawnCreep(Creep creep)
    {
        GameObject newCreep = new GameObject();
        newCreep = Instantiate(creep.prefab, new Vector3(startPos.x, 0.5f, startPos.z), Quaternion.identity) as GameObject;
        //creepsOnBoard.Add(newCreep);
    }
    public IEnumerator spawnWave(List<Creep> wave, float delay)
    {
        yield return new WaitForEndOfFrame();
        setPath();
        if (wave.Count > 0)
        {
            spawnCreep(wave[0]);
            wave.RemoveAt(0);
            yield return new WaitForSeconds(delay);
            yield return spawnWave(wave, delay);
        }
        else { yield return null; }
    }
}
