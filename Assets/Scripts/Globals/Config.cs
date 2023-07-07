using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public bool showDebugInfo;

    public int boardWidth;
    public int boardHeight;
    
    public int corralWidth;
    public int corralHeight;
    public int corralGap; // gap between corral and main board
    
    public List<Vector2Int> pathRoadMap;

    [SerializedDictionary("prefab", "spawnRate")]
    public SerializedDictionary<GameObject, float> grassCellPrefabs;
    
    [SerializedDictionary("prefab", "spawnRate")]
    public SerializedDictionary<GameObject, float> treeCellPrefabs;
    
    [SerializedDictionary("prefab", "spawnRate")]
    public SerializedDictionary<GameObject, float> dirtCellPrefabs;
    
    public List<Wave> waves;

    public float treeSpawnRate;

    public DeckPreset DeckPreset;
    public int startingCardAmount;

    public List<DeckPreset> shopLevelCardPresets;
    public List<FloatList> shopLevelCardSpawnRates;

    public int shopSize;
    public int rollShopCost;
    public List<int> shopCostToLevelUpAtLevel;
    public int amountToDecreaseShopLevelUpCostPerTurn;

    public int startingIncome;
    public int startingMoney;
    public int startingHealth;
    
    public List<CardPreset> turnEffectsToPlayOnATurn = new List<CardPreset>();
}
