using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target; // Obiectul față de care se menține distanța
    [SerializeField] private float distanceZ = 5f; // Distanța constantă pe axa Z

    [Header("NPC Spawning Settings")]
    [SerializeField] private List<GameObject> npcDefaultPrefabs; // Lista de prefab-uri NPC
    [SerializeField] private List<GameObject> npcOppositePrefabs;
    [SerializeField] private Transform spawnPoint; // Punctul de spawn implicit

    [Header("Spawn Rate Settings")]
    [SerializeField] private float defaultSpawnRate = 2f; // Intervalul pentru spawn default
    [SerializeField] private float customSpawnRate = 3f; // Intervalul pentru spawn custom

    private Vector3 offset;
    private bool isSpawningDefault = false; // Controlează corutina de spawn default
    private bool isSpawningCustom = false; // Controlează corutina de spawn custom

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target nu este asignat!");
            return;
        }
        
        offset = transform.position - target.position;

        // Pornim spawn-ul pentru ambele tipuri
        StartDefaultSpawning();
        StartCustomSpawning();
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // Menține poziția pe axa Z constantă față de target
            Vector3 targetPosition = target.position + offset;
            targetPosition.z = target.position.z + distanceZ;

            transform.position = new Vector3(transform.position.x, transform.position.y, targetPosition.z);
        }
    }

    /// <summary>
    /// Spawnează un NPC cu configurația implicită.
    /// </summary>
    private void SpawnDefaultNPC()
    {
        if (npcDefaultPrefabs.Count == 0)
        {
            Debug.LogWarning("Nu există prefab-uri în listă!");
            return;
        }

        int randomIndex = Random.Range(0, npcDefaultPrefabs.Count);
        Instantiate(npcDefaultPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
    }

    /// <summary>
    /// Spawnează un NPC cu rotația pe Y la 180 și poziția pe X la -11.16.
    /// </summary>
    private void SpawnOppositeNPC()
    {
        if (npcOppositePrefabs.Count == 0)
        {
            Debug.LogWarning("Nu există prefab-uri în listă!");
            return;
        }

        int randomIndex = Random.Range(0, npcOppositePrefabs.Count);

        Vector3 customPosition = new Vector3(-11.16f, spawnPoint.position.y, spawnPoint.position.z);

        Instantiate(npcOppositePrefabs[randomIndex], customPosition, npcOppositePrefabs[randomIndex].transform.rotation);
    }

    /// <summary>
    /// Pornește spawn-ul pentru NPC-urile default.
    /// </summary>
    public void StartDefaultSpawning()
    {
        if (!isSpawningDefault)
        {
            isSpawningDefault = true;
            StartCoroutine(DefaultSpawnCoroutine());
        }
    }

    /// <summary>
    /// Pornește spawn-ul pentru NPC-urile custom.
    /// </summary>
    public void StartCustomSpawning()
    {
        if (!isSpawningCustom)
        {
            isSpawningCustom = true;
            StartCoroutine(CustomSpawnCoroutine());
        }
    }

    /// <summary>
    /// Oprește spawn-ul pentru NPC-urile default.
    /// </summary>
    public void StopDefaultSpawning()
    {
        isSpawningDefault = false;
        StopCoroutine(DefaultSpawnCoroutine());
    }

    /// <summary>
    /// Oprește spawn-ul pentru NPC-urile custom.
    /// </summary>
    public void StopCustomSpawning()
    {
        isSpawningCustom = false;
        StopCoroutine(CustomSpawnCoroutine());
    }

    /// <summary>
    /// Corutina pentru spawn default.
    /// </summary>
    private IEnumerator DefaultSpawnCoroutine()
    {
        while (isSpawningDefault)
        {
            SpawnDefaultNPC();
            yield return new WaitForSeconds(defaultSpawnRate);
        }
    }

    /// <summary>
    /// Corutina pentru spawn custom.
    /// </summary>
    private IEnumerator CustomSpawnCoroutine()
    {
        while (isSpawningCustom)
        {
            SpawnOppositeNPC();
            yield return new WaitForSeconds(customSpawnRate);
        }
    }
}
