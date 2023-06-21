using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsUtils 
{
    // Get height of 3d game object
    public static float GetHeightOf(GameObject gameObject)
    {
        return gameObject.GetComponent<Renderer>().bounds.size.y;
    }
    
    // Get the center, top of 3d game object
    public static Vector3 GetTopOf(GameObject gameObject)
    {
        var position = gameObject.transform.position;

        return new Vector3(position.x, position.y + GetHeightOf(gameObject), position.z);
    }
    
}
