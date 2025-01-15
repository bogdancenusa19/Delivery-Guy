using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioSource audioSource;
    private float savedTime;

    private void Start()
    {
        slider.value = PlayerPrefs.GetFloat("audio");
        audioSource.volume = slider.value;
        audioSource.time = PlayerPrefs.GetFloat("clipTime");
    }

    private void Update()
    {
        if (audioSource.isPlaying)
        {
            savedTime = audioSource.time;
            PlayerPrefs.SetFloat("clipTime", savedTime);
        }
        
        audioSource.volume = slider.value;
        PlayerPrefs.SetFloat("audio", slider.value);
    }
}
