using UnityEngine;

[CreateAssetMenu(fileName = "CreepPreset", menuName = "ScriptableObjects/CreepPreset", order = 1)]
public class CreepPreset : ScriptableObject
{
    public GameObject prefab;
    public float moveSpeed;

}