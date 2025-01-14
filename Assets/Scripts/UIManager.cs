using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tips;
    [SerializeField] private TextMeshProUGUI areaStop;
    [SerializeField] private Slider boost;

    private int currentAreasNumber = 3;


    private void Start()
    {
        boost.value = boost.maxValue;
    }

    public void UpdateTips(string text)
    {
        tips.text = text + "$";
    }

    public void UpdateArea(int value)
    {
        areaStop.text = "Area Stop: " + (value + currentAreasNumber);
    }

    public void UpdateBoost(float value)
    {
        boost.value = value;
    }
}
