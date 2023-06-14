using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameEngine : MonoBehaviour
{
    [SerializeField] public int boardWidth;
    [SerializeField] public int boardHeight;
    
    public GameObject pfCell; // Cell to use on the board // CONSIDER FACTORING OUT TO SOME SETTINGS THING
    
    public Camera mainCamera;
    
    
    private Board _board;
    // Start is called before the first frame update
    void Start()
    {
        // Create a GameObject and attach the Board script to it
        GameObject boardObject = new GameObject("Board");
        
        // Attach the board script to the boardObject
        _board = boardObject.AddComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}