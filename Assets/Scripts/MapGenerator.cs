using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] zones; // Array-ul cu zonele curente din scenă
    [SerializeField] private GameObject[] zonePrefabs; // Array cu prefabricatele posibile
    [SerializeField] private GameObject[] destinationPrefabs; // Array cu prefabricatele pentru sectiuni de destinatie
    [SerializeField] private int startIndex = 3; // Indexul minim de la care poate fi selectată destinația

    public float zoneLength = 143f; // Lungimea fixă a fiecărei zone
    public int targetIndex { get; set; } // Indexul destinatiei
    private int sectionCounter = 0;

    void Start()
    {
        // Determină un index random pentru destinație
        targetIndex = Random.Range(startIndex, 10); 
        Debug.Log("Destinația a fost setată pe indexul: " + targetIndex);
        
        // Calculează lungimea zonei pe baza distanței dintre primele două zone
        if (zones.Length > 1)
        {
            zoneLength = zones[1].transform.position.z - zones[0].transform.position.z;
        }
        else
        {
            Debug.LogWarning("Nu sunt suficiente zone pentru a calcula lungimea.");
        }
    }

    public void OnEnterZone()
    {
        RemoveAndShiftZones();
        SpawnNewZoneAtEnd();
    }

    void RemoveAndShiftZones()
    {
        // Șterge prima zonă
        Destroy(zones[0]);

        // Shift la zone (toate își schimbă poziția în array cu -1)
        for (int i = 1; i < zones.Length; i++)
        {
            zones[i - 1] = zones[i];
        }
    }

    void SpawnNewZoneAtEnd()
    {
        GameObject newZonePrefab;
        sectionCounter++;

        // Verifică dacă zona curentă este cea de pe indexul destinatiei
        if (sectionCounter == targetIndex)
        {
            // Alege din prefabricatele pentru destinație
            newZonePrefab = destinationPrefabs[Random.Range(0, destinationPrefabs.Length)];
            Debug.Log("A fost instanțiată o zonă de destinație.");
        }
        else
        {
            // Alege o zonă random din prefabricatele clasice
            newZonePrefab = zonePrefabs[Random.Range(0, zonePrefabs.Length)];
        }

        // Determină poziția ultimei zone și adaugă lungimea fixă a zonei
        GameObject lastZone = zones[zones.Length - 2]; // Ultima zonă activă din array
        float spawnZ = lastZone.transform.position.z + zoneLength;

        // Instanțiază noua zonă la poziția calculată
        Vector3 spawnPosition = new Vector3(lastZone.transform.position.x, lastZone.transform.position.y, spawnZ);
        GameObject newZone = Instantiate(newZonePrefab, spawnPosition, Quaternion.identity);

        // Setează noua zonă ca și child al obiectului curent
        newZone.transform.parent = this.transform;

        // Adaugă noua zonă la finalul array-ului
        zones[zones.Length - 1] = newZone;
    }
}