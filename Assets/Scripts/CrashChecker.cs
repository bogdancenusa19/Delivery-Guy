using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashChecker : MonoBehaviour
{
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
}
