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
    [SerializeField] private int startIndex = 3; // Indexul minim de la care poate fi selectată destinația
    public int targetIndex { get; private set; } // Indexul destinației
    private float timeToDestination = 0; // Timpul alocat pentru a ajunge la destinație
    private float destinationTime; // Ora destinației în minute
    private float arrivalTime; // Timpul efectiv de sosire

    [System.Serializable] public struct TipOption
    {
        public float percentage; // Procentajul șansei
        public int value;        // Valoarea bacșișului
    }

    [Header("Tip Calculation")]
    [SerializeField] private TipOption[] tipOptions; // Opțiuni pentru bacșiș
    public bool hasReachedDestination { get; set; } = false;
    private bool tipsReceived = false;

    [Header("Map Settings")]
    [SerializeField] private GameObject[] zones; // Array-ul cu zonele curente din scenă
    [SerializeField] private GameObject[] zonePrefabs; // Prefabricatele pentru zone clasice
    [SerializeField] private GameObject[] destinationPrefabs; // Prefabricatele pentru secțiuni de destinație
    [SerializeField] private float zoneLength = 99f; // Lungimea fixă a fiecărei zone
    private int sectionCounter = 0; // Contor pentru secțiuni generate

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
        uiManager.UpdateBoost(player.stamina);
        if(!tipsReceived && hasReachedDestination)
            GiveTip();
    }

    private void InitializeDestination()
    {
        targetIndex = Random.Range(startIndex, 10);
        uiManager.UpdateArea(targetIndex);
        timeToDestination = targetIndex * 10;
        Debug.Log($"Destinatia este in {timeToDestination} secunde.");

        destinationTime = Random.Range(17f * 60, 18f * 60);
        string destinationTimeString = ConvertTimeToString(destinationTime);
        
        Debug.Log($"Destinația a fost setată pe indexul: {targetIndex}");
    }

    private void InitializeMapZones()
    {
        if (zones.Length > 1)
        {
            zoneLength = zones[1].transform.position.z - zones[0].transform.position.z;
        }
        else
        {
            Debug.LogWarning("Nu sunt suficiente zone pentru a calcula lungimea.");
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
            Debug.Log($"A fost instanțiată o zonă de destinație la secțiunea {sectionCounter}.");
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
}
