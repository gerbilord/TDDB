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
    public int currentTurnNumber { get; set; }
    
    public void SendCreepToEnemyCorral(CreepPreset creepToSend)
    {
        GlobalVariables.playerGameEngine.waveManager.AddCreepToCorral(creepToSend);
    }

    public void SendCreepToEnemyImmediateSend(CreepPreset creepToSend)
    {
        GlobalVariables.playerGameEngine.waveManager.AddCreepToSendImmediate(creepToSend);
    }


    public void FinishCardTurn_StartWave()
    {
        waveManager.SpawnWave(currentTurnNumber);
    }

    public void OnWaveEnd_StartCardTurn()
    {
        currentTurnNumber += 1;
        PlayNextTurnEffects();
    }

    public void PlayNextTurnEffects()
    {
        if(currentTurnNumber > config.turnEffectsToPlayOnATurn.Count)
            return;

        List<IPlayEffects> effectsToPlay = config.turnEffectsToPlayOnATurn[currentTurnNumber-1].GetPlayEffects();
        
        foreach (IPlayEffects playEffect in effectsToPlay)
        {
            playEffect.gameEngine = this;
            playEffect.Play(); // This doesn't trigger events currently.
        }
    }


    public void Setup()
    {
        currentTurnNumber = 1;
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