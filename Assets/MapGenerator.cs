using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] zones; // Array-ul cu zonele curente din scenă
    public GameObject[] zonePrefabs; // Array cu prefabricatele posibile
    public float zoneLength = 143f; // Lungimea fixă a fiecărei zone

    void Start()
    {
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
        // Alege o zonă random din prefabricatele disponibile
        GameObject newZonePrefab = zonePrefabs[Random.Range(0, zonePrefabs.Length)];

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