using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesUIUpdate : MonoBehaviour
{
    private CrashChecker crashChecker;

    [SerializeField] private Image[] livesImages;

    private void Awake()
    {
        crashChecker = FindObjectOfType<CrashChecker>().GetComponent<CrashChecker>();
    }

    private void Update()
    {
        switch (crashChecker.GetLives())
        {
            case 2:
                livesImages[livesImages.Length - 1].color = Color.black;
                break;
            case 1:
                livesImages[livesImages.Length - 1].color = Color.black;
                livesImages[^2].color = Color.black;
                break;
        }
        
    }
}
