using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radio : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public AudioSource audioSource;
    public Slider volumeSlider;

    public float volumeStep = 0.1f;
    public float volumeChangeSpeed = 0.1f;

    private bool isIncreasingVolume = false;
    private bool isDecreasingVolume = false;

    private void Start()
    {
        volumeSlider.value = audioSource.volume;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeClip();
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            isDecreasingVolume = true;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            isIncreasingVolume = true;
        }
        else
        {
            isDecreasingVolume = false;
            isIncreasingVolume = false;
        }
        
        if (isDecreasingVolume)
        {
            AdjustVolume(-volumeChangeSpeed * Time.deltaTime);
        }
        else if (isIncreasingVolume)
        {
            AdjustVolume(volumeChangeSpeed * Time.deltaTime);
        }
    }

    private void ChangeClip()
    {
        List<AudioClip> availableClips = audioClips.FindAll(clip => clip != audioSource.clip);
        
        int randomIndex = Random.Range(0, availableClips.Count);
        audioSource.clip = availableClips[randomIndex];
        audioSource.Play();
    }

    private void AdjustVolume(float change)
    {
        audioSource.volume = Mathf.Clamp(audioSource.volume + change, 0f, 1f);
        volumeSlider.value = audioSource.volume;
    }
}
