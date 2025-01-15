using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    private UIManager uiManager;
    [SerializeField] public PlayerCarBehavior player;

    [Header("Delivery Settings")]
    [SerializeField] private int startIndex = 3;
    public int targetIndex { get; private set; }
    private float timeToDestination = 0; 
    private float destinationTime;
    private float arrivalTime; 
    private float helpTime = 20f;
    private int currentArea = 1;

    [System.Serializable] public struct TipOption
    {
        public float percentage; 
        public int value;
    }

    [Header("Tip Calculation")]
    [SerializeField] private TipOption[] tipOptions; 
    public bool hasReachedDestination { get; set; } = false;
    private bool tipsReceived = false;

    [Header("Map Settings")]
    [SerializeField] private GameObject[] zones;
    [SerializeField] private GameObject[] zonePrefabs; 
    [SerializeField] private GameObject[] destinationPrefabs;
    [SerializeField] private float zoneLength = 99f;
    private int sectionCounter = 0;

    private void Awake()
    {
        uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        InitializeDestination();
        InitializeMapZones();
        StartCoroutine(CountDownToDestination());
    }

    private void Update()
    {
        UpdateUI();
        
        if(!tipsReceived && hasReachedDestination)
            GiveTip();
    }

    private void UpdateUI()
    {
        uiManager.UpdateBoost(player.stamina);
        uiManager.UpdateTime(timeToDestination);
        uiManager.UpdateCurrentArea(currentArea);
    }

    private void InitializeDestination()
    {
        targetIndex = Random.Range(startIndex, 10);
        uiManager.UpdateArea(targetIndex);
        timeToDestination = targetIndex * 7 + helpTime;
        Debug.Log($"Destinatia este in {timeToDestination} secunde.");

        destinationTime = Random.Range(17f * 60, 18f * 60);
        string destinationTimeString = ConvertTimeToString(destinationTime);
        
        Debug.Log($"Destinatia a fost setata pe indexul: {targetIndex}");
    }

    private void InitializeMapZones()
    {
        if (zones.Length > 1)
        {
            zoneLength = zones[1].transform.position.z - zones[0].transform.position.z;
        }
    }
    private string ConvertTimeToString(float timeInMinutes)
    {
        int hours = Mathf.FloorToInt(timeInMinutes / 60);
        int minutes = Mathf.FloorToInt(timeInMinutes % 60);
        return $"{hours:D2}:{minutes:D2}";
    }

    public void OnEnterZone()
    {
        RemoveAndShiftZones();
        SpawnNewZoneAtEnd();
    }

    private void RemoveAndShiftZones()
    {
        Destroy(zones[0]);

        for (int i = 1; i < zones.Length; i++)
        {
            zones[i - 1] = zones[i];
        }
    }

    private void SpawnNewZoneAtEnd()
    {
        GameObject newZonePrefab;

        sectionCounter++;

        if (sectionCounter == targetIndex)
        {
            newZonePrefab = destinationPrefabs[Random.Range(0, destinationPrefabs.Length)];
        }
        else
        {
            newZonePrefab = zonePrefabs[Random.Range(0, zonePrefabs.Length)];
        }

        GameObject lastZone = zones[zones.Length - 2];
        float spawnZ = lastZone.transform.position.z + zoneLength;

        Vector3 spawnPosition = new Vector3(lastZone.transform.position.x, lastZone.transform.position.y, spawnZ);
        GameObject newZone = Instantiate(newZonePrefab, spawnPosition, Quaternion.identity);
        newZone.transform.parent = this.transform;

        zones[zones.Length - 1] = newZone;
    }

    IEnumerator CountDownToDestination()
    {
        while (timeToDestination > 0f)
        {
            timeToDestination -= Time.deltaTime;
            yield return null;
        }

        arrivalTime = destinationTime - Random.Range(0f, 30f);
    }

    private void GiveTip()
    {
        tipsReceived = true;
        float randomChance = Random.Range(0f, 1f);
        float cumulativeProbability = 0f;

        foreach (var tipOption in tipOptions)
        {
            cumulativeProbability += tipOption.percentage;
            if (randomChance < cumulativeProbability)
            {
                Debug.Log($"You received ${tipOption.value} as a tip!");
                uiManager.UpdateTips(tipOption.value.ToString());
                PlayerPrefs.SetInt("cash",PlayerPrefs.GetInt("cash") + tipOption.value);
                break;
            }
        }
    }

    public void EnterNewArea()
    {
        currentArea++;
    }
}
