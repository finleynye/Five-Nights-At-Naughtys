using TMPro;
using UnityEngine;
using System;

public class GetPlayerTime : MonoBehaviour
{
    [SerializeField] private TMP_Text date;
    [SerializeField] private NightCycle mainTime;
    [SerializeField] private TMP_Text thisTime;
    
    private void Update()
    {
        date.text = DateTime.Now.ToString();
        thisTime.text = mainTime.time.text;
    }
}
