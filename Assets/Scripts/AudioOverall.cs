using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOverall : MonoBehaviour
{

    private void Awake()
    {
        gameObject.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("audio");
    }
    
}
