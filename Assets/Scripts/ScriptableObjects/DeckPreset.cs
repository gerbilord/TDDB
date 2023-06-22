using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckPreset", menuName = "ScriptableObjects/DeckPreset", order = 1)]
public class DeckPreset : ScriptableObject
{
    public List<CardPreset> cards;

}