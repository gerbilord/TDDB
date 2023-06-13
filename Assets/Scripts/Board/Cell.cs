using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    // Field: List of game objects on cell
    private List<GameObject> _occupyingGameObjects;
    
    public bool isBuildable;
    public bool isPath;

    public int x;
    public int y;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateRender()
    {
        // if isPath is true change the material color to sand
        if (isPath)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else if (isBuildable)
        {
            // dark green color
            GetComponent<Renderer>().material.color = new Color(0.1f, 0.5f, 0.1f);
        }
    }
    
    
}
