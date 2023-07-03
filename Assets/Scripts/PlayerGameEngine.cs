using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class PlayerGameEngine : MonoBehaviour, IGameEngine
{
    public Board board { get; set; }
    public Player player { get; set; }
    public CardManager cardManager { get; set; }
    public WaveManager waveManager { get; set; }
    public Config config { get; set; }
    
    public int currentTurnNumber { get; set; }
    
    public void SendCreepToEnemyCorral(CreepPreset creepToSend)
    {
        GlobalVariables.enemyGameEngine.waveManager.AddCreepToCorral(creepToSend);
    }

    public void SendCreepToEnemyImmediateSend(CreepPreset creepToSend)
    {
        GlobalVariables.enemyGameEngine.waveManager.AddCreepToSendImmediate(creepToSend);
    }

    public void FinishCardTurn_StartWave()
    {
        waveManager.SpawnWave(currentTurnNumber);
    }

    public void OnWaveEnd_StartCardTurn()
    {
        currentTurnNumber += 1;
        GlobalVariables.uiManager.UpdateLevelIndicatorUI();
        player.AddIncomeToMoney();
        cardManager.MoveUsedCardsToDiscard();
        cardManager.DiscardHand();
        cardManager.DrawCards(config.startingCardAmount);
    }


    void Start()
    {
        currentTurnNumber = 1;
        GlobalVariables.mainCamera = Camera.main;

        GlobalVariables.playerGameEngine = this;
        
        config = GameObject.Find("PlayerConfig").GetComponent<Config>();

        GlobalVariables.eventManager = new EventManager();

        player = new Player(this);

        GlobalVariables.uiManager = new UIManager();

        cardManager = new CardManager(this);
        
        cardManager.LoadDeck();
        cardManager.LoadShop();
        cardManager.StartTurn();

        // Create an empty GameObject and attach the Board script to it
        GameObject boardObject = new GameObject("Board");
        board = boardObject.AddComponent<Board>();
        board.Setup(CorralPosition.BottomRight, this);
        GlobalVariables.uiManager.SetupCamera();
        
        // Create an empty GameObject and attach the wave manager script to it
        GameObject waveManagerObject = new GameObject("WaveManager");
        waveManager = waveManagerObject.AddComponent<WaveManager>();
        waveManager.Setup(this);
        
        EnemyGameEngine enemyGameEngine = this.GetComponent<EnemyGameEngine>();
        enemyGameEngine.Setup();
        
        GlobalVariables.uiManager.Setup();
        
        // Play our first turn effects
        enemyGameEngine.PlayNextTurnEffects();
    }
}