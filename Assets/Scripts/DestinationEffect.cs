using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestinationEffect : MonoBehaviour
{
    private LevelSFX levelSfx;

    private void Awake()
    {
        levelSfx = FindObjectOfType<LevelSFX>().GetComponent<LevelSFX>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            levelSfx.PlayArriveClip();
            other.GetComponent<PlayerCarBehavior>().currentSpeed = 0;
        }
    }
}
