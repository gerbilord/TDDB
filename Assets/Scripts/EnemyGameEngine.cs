using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class EnemyGameEngine : MonoBehaviour, IGameEngine
{
    public Board board { get; set; }
    public Player player { get; set; } // This is actually the enemy stats!
    public CardManager cardManager { get; set; }
    public WaveManager waveManager { get; set; }
    public Config config { get; set; }
    
    public void Setup()
    {
        GlobalVariables.enemyGameEngine = this;
        // config = GameObject.Find("EnemyConfig").GetComponent<Config>();
        config = GameObject.Find("PlayerConfig").GetComponent<Config>();
        
        player = new Player(this);
        cardManager = null; // TODO Add AI card manager
        
        // Create an empty GameObject and attach the Board script to it
        GameObject boardObject = new GameObject("Board");
        board = boardObject.AddComponent<Board>();
        board.Setup(CorralPosition.TopLeft, this);
        board.transform.position = new Vector3(GlobalVariables.playerGameEngine.config.boardWidth + GlobalVariables.playerGameEngine.config.corralGap * 2 + GlobalVariables.playerGameEngine.config.corralWidth, 0, 0);

        // Create an empty GameObject and attach the wave manager script to it
        GameObject waveManagerObject = new GameObject("WaveManager");
        waveManager = waveManagerObject.AddComponent<WaveManager>();
        waveManager.Setup(this);
    }
}