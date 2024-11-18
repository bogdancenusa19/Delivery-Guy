using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tips;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI deadline;


    public void UpdateTips(string text)
    {
        tips.text = text + "$";
    }

    public void UpdateTime(string text)
    {
        time.text = "Time: " + text;
    }

    public void UpdateDeadline(string text)
    {
        deadline.text = "Deadline: " + text;
    }
}
