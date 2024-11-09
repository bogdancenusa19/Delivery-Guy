using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceChecker : MonoBehaviour
{
    private MapGenerator mapGenerator;

    private void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>().GetComponent<MapGenerator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            mapGenerator.OnEnterZone();
    }
}
