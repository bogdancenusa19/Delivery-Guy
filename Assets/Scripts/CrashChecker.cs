using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashChecker : MonoBehaviour
{
    [SerializeField] private LevelSFX levelSfx;
    private int lives = 3;
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("VehicleOpposite"))
        {
            GameOver();
        }
        else if (other.gameObject.CompareTag("Vehicle"))
        {
            lives--;
            levelSfx.PlayHornClip();
            
            if (lives == 0)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        SceneManager.LoadScene(0);
    }

    public int GetLives()
    {
        return lives;
    }
}
