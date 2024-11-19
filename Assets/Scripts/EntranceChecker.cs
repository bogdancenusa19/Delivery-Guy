using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceChecker : MonoBehaviour
{
    private GameManager _gameManager;
    private bool playerAtDestination = false;
    
    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    private void Update()
    {
        if(playerAtDestination)
            CheckIfParked();
    }

    private void CheckIfParked()
    {
        if (_gameManager.player.currentSpeed == 0)
        {
            _gameManager.hasReachedDestination = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && this.gameObject.CompareTag("Destination"))
        {
            playerAtDestination = true;
            PlayerCarBehavior player = other.GetComponent<PlayerCarBehavior>();
            player.StopAtDestination();
        }
        else if(other.CompareTag("Player"))
        {
            _gameManager.OnEnterZone();
        }
    }
}
