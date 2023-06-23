using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BulletPreset", menuName = "ScriptableObjects/BulletPreset", order = 1)]
public class BulletPreset : ScriptableObject
{
    [SerializedDictionary("StatType", "value")]
    public SerializedDictionary<StatType,float> stats;
    
    public GameObject bulletPrefab;

    public IBullet makeBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab);
        Dictionary<StatType, float> newStats = new Dictionary<StatType, float>(stats);
        newBullet.GetComponent<IBullet>().stats = newStats;
        return newBullet.GetComponent<IBullet>();
    }

}