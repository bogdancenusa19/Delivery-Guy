using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target; 
    [SerializeField] private float distanceZ = 5f;

    [Header("NPC Spawning Settings")]
    [SerializeField] private List<GameObject> npcDefaultPrefabs;
    [SerializeField] private List<GameObject> npcOppositePrefabs;
    [SerializeField] private Transform spawnPoint;

    [Header("Spawn Rate Settings")]
    [SerializeField] private float defaultSpawnRate = 2f;
    [SerializeField] private float customSpawnRate = 3f;

    private Vector3 offset;
    private bool isSpawningDefault = false;
    private bool isSpawningCustom = false;

    private void Start()
    {
        offset = transform.position - target.position;
        
        StartDefaultSpawning();
        StartCustomSpawning();
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            targetPosition.z = target.position.z + distanceZ;

            transform.position = new Vector3(transform.position.x, transform.position.y, targetPosition.z);
        }
    }
    private void SpawnDefaultNPC()
    {
        int randomIndex = Random.Range(0, npcDefaultPrefabs.Count);
        Instantiate(npcDefaultPrefabs[randomIndex], spawnPoint.position, Quaternion.identity);
    }
    private void SpawnOppositeNPC()
    {
        int randomIndex = Random.Range(0, npcOppositePrefabs.Count);

        Vector3 customPosition = new Vector3(-11.16f, spawnPoint.position.y, spawnPoint.position.z);

        Instantiate(npcOppositePrefabs[randomIndex], customPosition, npcOppositePrefabs[randomIndex].transform.rotation);
    }
    
    public void StartDefaultSpawning()
    {
        if (!isSpawningDefault)
        {
            isSpawningDefault = true;
            StartCoroutine(DefaultSpawnCoroutine());
        }
    }
    public void StartCustomSpawning()
    {
        if (!isSpawningCustom)
        {
            isSpawningCustom = true;
            StartCoroutine(CustomSpawnCoroutine());
        }
    }
    
    public void StopDefaultSpawning()
    {
        isSpawningDefault = false;
        StopCoroutine(DefaultSpawnCoroutine());
    }
    
    public void StopCustomSpawning()
    {
        isSpawningCustom = false;
        StopCoroutine(CustomSpawnCoroutine());
    }
    
    private IEnumerator DefaultSpawnCoroutine()
    {
        while (isSpawningDefault)
        {
            SpawnDefaultNPC();
            yield return new WaitForSeconds(defaultSpawnRate);
        }
    }
    
    private IEnumerator CustomSpawnCoroutine()
    {
        while (isSpawningCustom)
        {
            SpawnOppositeNPC();
            yield return new WaitForSeconds(customSpawnRate);
        }
    }
}
