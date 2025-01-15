using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSFX : MonoBehaviour
{
    [SerializeField] private AudioClip arriveClip;
    [SerializeField] private AudioClip hornClip;
    
    public float checkInterval = 2f;
    
    private void Start()
    {
        StartCoroutine(PlayHornRandomly());
    }
    
    private IEnumerator PlayHornRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);
            
            int randomValue = Random.Range(1, 6);
            if (randomValue == 1)
            {
                GetComponent<AudioSource>().PlayOneShot(hornClip);
            }
        }
    }
    
    public void PlayArriveClip()
    {
        GetComponent<AudioSource>().PlayOneShot(arriveClip);
    }

    public void PlayHornClip()
    {
        GetComponent<AudioSource>().PlayOneShot(hornClip);
    }
}
