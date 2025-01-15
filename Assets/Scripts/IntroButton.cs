using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroButton : MonoBehaviour
{
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private TextMeshProUGUI intro1;
    [SerializeField] private TextMeshProUGUI intro2;
    [SerializeField] private GameObject intro3;
    [SerializeField] private GameObject intro4;

    [SerializeField] private Button button;
    private int pressNumber = 0;
    void Start()
    {
        intro1.gameObject.SetActive(true);
    }

    public void PressButton()
    {
        switch (pressNumber)
        {
            case 0:
                intro1.gameObject.SetActive(false);
                intro2.gameObject.SetActive(true);
                pressNumber++;
                break;
            
            case 1:
                intro2.gameObject.SetActive(false);
                intro3.gameObject.SetActive(true);
                pressNumber++;
                break;
            
            case 2:
                intro3.gameObject.SetActive(false);
                intro4.gameObject.SetActive(true);
                button.gameObject.SetActive(false);
                pressNumber++;
                break;
            
            default:
                break;
        }
    }

    public void CloseInfo()
    {
        intro1.gameObject.SetActive(true);
        intro2.gameObject.SetActive(false);
        intro3.gameObject.SetActive(false);
        intro4.gameObject.SetActive(false);
        button.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        menuCanvas.SetActive(true);
    }
}
