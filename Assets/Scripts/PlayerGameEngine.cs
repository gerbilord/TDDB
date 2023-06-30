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

    void Start()
    {
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
        
        this.GetComponent<EnemyGameEngine>().Setup();
        
        GlobalVariables.uiManager.Setup();
    }
}