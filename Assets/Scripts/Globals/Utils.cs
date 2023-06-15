using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class Utils
{
    // Get a random item from a List<>
    public static T GetRandomItem<T>(List<T> list)
    {
        // get random number within the range of the list index
        int randomIndex = UnityEngine.Random.Range(0, list.Count);

        // return the random item
        return list[randomIndex];
    }
    
    public static GameObject GetRateRandomItem(List<GameObject> list)
    {
        // Calculate the cumulative spawn rates
        List<float> cumulativeRates = new List<float>();
        float cumulativeRate = 0f;
        foreach (var item in list)
        {
            // Get component SpawnRate
            SpawnRate spawnRate = item.GetComponent<SpawnRate>();
            cumulativeRate += spawnRate.spawnRate;
            cumulativeRates.Add(cumulativeRate);
        }

        // Generate a random number between 0 and the total spawn rate
        Random random = new Random();
        float randomNumber = (float)random.NextDouble() * cumulativeRate;

        // Find the item corresponding to the generated random number
        GameObject selectedItem = default;
        for (int i = 0; i < cumulativeRates.Count; i++)
        {
            if (randomNumber < cumulativeRates[i])
            {
                selectedItem = list[i];
                break;
            }
        }

        return selectedItem;
    }
}