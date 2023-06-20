using UnityEngine;

[CreateAssetMenu(fileName = "Creep", menuName = "ScriptableObjects/Creep", order = 1)]
public class Creep : ScriptableObject
{
    public float rate = 0.5f;
    public GameObject prefab;
}