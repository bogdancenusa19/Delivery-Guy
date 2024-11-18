using UnityEngine;
using System.Collections;

public class DeliveryTimer : MonoBehaviour
{
    private float timeToDestination = 30f; // 30 seconds until destination
    private float destinationTime;
    private float arrivalTime;

    // Probabilities for tips
    private float[] tipPercentages = { 0.10f, 0.01f, 0.20f, 0.30f, 0.39f };
    private int[] tipValues = { 200, 500, 100, 50, 10 };
    
    void Start()
    {
        // Generate random destination time between 17:00 and 18:00 (in minutes)
        destinationTime = Random.Range(17f * 60, 18f * 60); // Random time between 17:00 and 18:00
        
        // Start the coroutine to count down to destination
        StartCoroutine(CountDownToDestination());
    }

    IEnumerator CountDownToDestination()
    {
        while (timeToDestination > 0f)
        {
            timeToDestination -= Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Once the player has "arrived" at the destination, calculate tip
        arrivalTime = destinationTime - Random.Range(0f, 30f); // Random early arrival time

        // Calculate and give the tip
        GiveTip();
    }

    void GiveTip()
    {
        float randomChance = Random.Range(0f, 1f);
        float cumulativeProbability = 0f;

        for (int i = 0; i < tipPercentages.Length; i++)
        {
            cumulativeProbability += tipPercentages[i];
            if (randomChance < cumulativeProbability)
            {
                Debug.Log($"You received ${tipValues[i]} as a tip!");
                break;
            }
        }
    }
}