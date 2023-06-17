using UnityEngine;
using UnityEngine.EventSystems;

public class GameEngine : MonoBehaviour
{
    [SerializeField] public int boardWidth;
    [SerializeField] public int boardHeight;
    
    public Camera mainCamera;
    
    public Board board;
    public CardManager cardManager;
    
    void Start()
    {
        GlobalVariables.gameEngine = this;
        GlobalVariables.eventManager = new EventManager();
        GlobalVariables.uiManager = new UIManager();
        GlobalVariables.config = FindObjectOfType<Config>();
        
        cardManager = new CardManager();
        
        // Create a GameObject and attach the Board script to it
        GameObject boardObject = new GameObject("Board");
        board = boardObject.AddComponent<Board>();
    }
}