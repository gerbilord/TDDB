using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class RandomUtils
{
    // Get a random item from a List<>
    public static T GetRandomItem<T>(List<T> list)
    {
        // get random number within the range of the list index
        int randomIndex = UnityEngine.Random.Range(0, list.Count);

        // return the random item
        return list[randomIndex];
    }
    
    // Get a random item from a Dictionary<>
    public static T GetRateRandomItem<T>(Dictionary<T, float> dictionary)
    {
        
        // Get a random GameObject based on the spawn rates. GameObjects should have change proportional to their spawn rate.
        
        // Get the total spawn rate
        float totalSpawnRate = 0;
        foreach (KeyValuePair<T, float> pair in dictionary)
        {
            totalSpawnRate += pair.Value;
        }
        
        // Get a random number between 0 and the total spawn rate
        float random = UnityEngine.Random.Range(0, totalSpawnRate);
        
        // Loop through the dictionary
        foreach (KeyValuePair<T, float> pair in dictionary)
        {
            // Subtract the spawn rate from the random number
            random -= pair.Value;
            
            // If the random number is less than 0, return the GameObject
            if (random < 0)
            {
                return pair.Key;
            }
        }
        
        throw new System.Exception("No item was found");
    }
}