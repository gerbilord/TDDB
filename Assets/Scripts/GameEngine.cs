using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameEngine : MonoBehaviour
{
    public Camera mainCamera;
    
    public Board board;
    public Player player;
    public CardManager cardManager;
    public WaveManager waveManager;
    
    
    void Start()
    {
        GlobalVariables.config = FindObjectOfType<Config>();
        GlobalVariables.gameEngine = this;
        GlobalVariables.eventManager = new EventManager();
        
        player = new Player();

        GlobalVariables.uiManager = new UIManager();
        
        
        cardManager = new CardManager();
        cardManager.LoadDeck();
        cardManager.StartTurn();

        // Create an empty GameObject and attach the Board script to it
        GameObject boardObject = new GameObject("Board");
        board = boardObject.AddComponent<Board>();
        
        // Create an empty GameObject and attach the wave manager script to it
        GameObject waveManagerObject = new GameObject("WaveManager");
        waveManager = waveManagerObject.AddComponent<WaveManager>();
    }
}