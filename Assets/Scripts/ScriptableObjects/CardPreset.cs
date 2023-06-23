using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CardPreset", menuName = "ScriptableObjects/CardPreset", order = 1)]
public class CardPreset : ScriptableObject
{
    public GameObject cardPrefab;
    public TowerPreset towerPreset;
    
    public ICard makeCard()
    {
        GameObject newCard = Instantiate(cardPrefab);
        newCard.GetComponent<ICard>().towerPreset = towerPreset;
        return newCard.GetComponent<ICard>();
    }

}