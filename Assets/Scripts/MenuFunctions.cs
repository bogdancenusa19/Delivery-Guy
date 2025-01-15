using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cash;
    [SerializeField] private GameObject howToPlayCanvas;

    private void Start()
    {
        cash.text = PlayerPrefs.GetInt("cash").ToString() + "$";
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void HowToPlay()
    {
        this.gameObject.SetActive(false);
        howToPlayCanvas.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
