using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraphicsUtils 
{
    // Get height of 3d game object
    public static float GetHeightOf3d(GameObject gameObject)
    {
        return gameObject.GetComponentsInChildren<Renderer>().OrderBy(children => children.bounds.size.y).Last().bounds.size.y;
    }
    
    // Get the center, top of 3d game object
    public static Vector3 GetTopOf3d(GameObject gameObject)
    {
        var position = gameObject.transform.position;

        return new Vector3(position.x, position.y + GetHeightOf3d(gameObject), position.z);
    }

    public static Vector3 GetTowerShootSpawnPoint(ITower tower)
    {
        GameObject towerObj = tower.GetGameObject();
        
        // Get child called "ShootSpawnPoint" on towerObj
        GameObject shootSpawnPoint = towerObj.transform.Find("ShootSpawnPoint").gameObject;

        return shootSpawnPoint.transform.position;
    }
    
    
    public static float GetHeightOf2d(GameObject gameObject)
    {
        return gameObject.GetComponent<RectTransform>().rect.height * gameObject.transform.localScale.y;
    }
    
    public static float GetWidthOf2d(GameObject gameObject)
    {
        return gameObject.GetComponent<RectTransform>().rect.width * gameObject.transform.localScale.x;
    }

}
