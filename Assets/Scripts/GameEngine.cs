using UnityEngine;

public class GameEngine : MonoBehaviour
{
    [SerializeField] public int boardWidth;
    [SerializeField] public int boardHeight;
    
    public Camera mainCamera;
    
    private Board _board;
    
    void Start()
    {
        GlobalVariables.gameEngine = this;
        GlobalVariables.eventManager = new EventManager();
        GlobalVariables.uiManager = new UIManager();
        GlobalVariables.config = FindObjectOfType<Config>();
        
        // Create a GameObject and attach the Board script to it
        GameObject boardObject = new GameObject("Board");
        
        // Attach the board script to the boardObject
        _board = boardObject.AddComponent<Board>();
    }
}