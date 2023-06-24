using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameEngine : MonoBehaviour
{
    public Camera mainCamera;
    
    public Board board;
    public CardManager cardManager;
    public WaveManager waveManager;
    
    
    void Start()
    {
        GlobalVariables.gameEngine = this;
        GlobalVariables.eventManager = new EventManager();
        GlobalVariables.uiManager = new UIManager();
        GlobalVariables.config = FindObjectOfType<Config>();
        
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