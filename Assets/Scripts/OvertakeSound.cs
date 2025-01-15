using UnityEngine;

public class OvertakeSound : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip engineLoopClip; 
    public AudioClip accelerationClip; 

    public float accelerationPitchIncrease = 10;
    public float reversePitchDecreaseSpeed = 1f; 

    private bool isAccelerating = false;

    void Start()
    {
        if (audioSource == null)
        {
            Debug.LogError("AudioSource lipsește! Asociază-l în Inspector.");
            return;
        }
        
        audioSource.clip = engineLoopClip;
        audioSource.loop = true; 
        audioSource.Play();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isAccelerating)
            {
                StartAcceleration();
            }
            else
            {
                audioSource.pitch = Mathf.Lerp(audioSource.pitch, accelerationPitchIncrease, Time.deltaTime * 5f);
            }
        }
        else
        {
            if (isAccelerating)
            {
                StopAcceleration();
            }
            else
            {
                audioSource.pitch = Mathf.Lerp(audioSource.pitch, 1f, Time.deltaTime * reversePitchDecreaseSpeed);

                if (audioSource.pitch <= 1.05f && audioSource.clip != engineLoopClip)
                {
                    audioSource.clip = engineLoopClip;
                    audioSource.loop = true;
                    audioSource.Play();
                }
            }
        }
    }

    private void StartAcceleration()
    {
        isAccelerating = true;
        audioSource.clip = accelerationClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    private void StopAcceleration()
    {
        isAccelerating = false;
    }
}
