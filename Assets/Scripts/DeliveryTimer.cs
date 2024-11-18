using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class DeliveryTimer : MonoBehaviour
{
    private UIManager uiManager;
    private MapGenerator mapGenerator;

    private float timeToDestination = 0;
    private float destinationTime;
    private float arrivalTime;

    // Probabilities for tips
    private float[] tipPercentages = { 0.10f, 0.01f, 0.20f, 0.30f, 0.39f };
    private int[] tipValues = { 200, 500, 100, 50, 10 };

    private void Awake()
    {
        uiManager = GetComponent<UIManager>();
        mapGenerator = FindObjectOfType<MapGenerator>().GetComponent<MapGenerator>();
    }

    void Start()
    {
        
        //TODO: DeliveryTimer + MapGenerater -> GameManager
        timeToDestination = mapGenerator.targetIndex * 10;
        Debug.Log("Destinatia este in " + timeToDestination);
        uiManager.UpdateDeadline(timeToDestination.ToString());
        
        // Generate random destination time between 17:00 and 18:00 (in minutes)
        destinationTime = Random.Range(17f * 60, 18f * 60); // Random time between 17:00 and 18:00
        string destinationTimeString = ConvertTimeToString(destinationTime);
        
        uiManager.UpdateTime(destinationTimeString);
        
        // Start the coroutine to count down to destination
        StartCoroutine(CountDownToDestination());
    }
    
    private  string ConvertTimeToString(float timeInMinutes)
    {
        int hours = Mathf.FloorToInt(timeInMinutes / 60); // Extract hours
        int minutes = Mathf.FloorToInt(timeInMinutes % 60); // Extract minutes
        return $"{hours:D2}:{minutes:D2}"; // Format as HH:MM
    }


    IEnumerator CountDownToDestination()
    {
        while (timeToDestination > 0f)
        {
            timeToDestination -= Time.deltaTime;
            uiManager.UpdateDeadline(timeToDestination.ToString("F0"));
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
                uiManager.UpdateTips(tipValues[i].ToString());
                break;
            }
        }
    }
}