using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CardPreset", menuName = "ScriptableObjects/CardPreset", order = 1)]
public class CardPreset : ScriptableObject
{
    public GameObject cardPrefab;
    public TowerPreset towerPreset;

}