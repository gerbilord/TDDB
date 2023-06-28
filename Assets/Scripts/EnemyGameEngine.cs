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
        config = CreateEnemyConfig();
        
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

    private Config CreateEnemyConfig()
    {
        // Some things we want to copy from the player config!
        Config enemyConfig = GameObject.Find("EnemyConfig").GetComponent<Config>();
        Config playerConfig = GameObject.Find("PlayerConfig").GetComponent<Config>();
        
        enemyConfig.boardWidth = playerConfig.boardWidth;
        enemyConfig.boardHeight = playerConfig.boardHeight;
        
        enemyConfig.corralWidth = playerConfig.corralWidth;
        enemyConfig.corralHeight = playerConfig.corralHeight;
        enemyConfig.corralGap = playerConfig.corralGap;
        
        enemyConfig.pathRoadMap = playerConfig.pathRoadMap;
        
        enemyConfig.waves = playerConfig.waves;

        return enemyConfig;
    }
}