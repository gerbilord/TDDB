using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "CardPreset", menuName = "ScriptableObjects/CardPreset", order = 1)]
public class CardPreset : ScriptableObject
{
    public GameObject cardPrefab;
    public List<ScriptableObject> playEffectsList; // TODO change to List<IPlayEffects>. IPlayEffects should be an abstract class that extends ScriptableObjects.

    public ICard makeCard()
    {
        // Map playEffectsList to List<IPlayEffects>
        List<IPlayEffects> playEffectsList = new List<IPlayEffects>();
        foreach (ScriptableObject playEffect in this.playEffectsList)
        {
            playEffectsList.Add((IPlayEffects)playEffect);
        }

        GameObject newCard = Instantiate(cardPrefab);
        newCard.GetComponent<ICard>().playEffects = playEffectsList;
        return newCard.GetComponent<ICard>();
    }

}