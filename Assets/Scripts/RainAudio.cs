using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainAudio : MonoBehaviour
{
    private void Start()
    {
        GetComponent<AudioSource>().PlayDelayed(3f);
    }
}
