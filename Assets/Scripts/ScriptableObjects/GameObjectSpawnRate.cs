using UnityEngine;

[CreateAssetMenu(fileName = "newGameObjectSpawnRate", menuName = "ScriptableObjects/GameObjectSpawnRate", order = 1)]
public class GameObjectSpawnRate : ScriptableObject
{
    public float rate = 0.5f;
    public GameObject prefab;
}