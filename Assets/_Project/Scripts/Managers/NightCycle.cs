using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NightCycle : MonoBehaviour
{
    public float nightDuration; //Seconds, 360 / 6 = 6 minutes 12-6AM
    public int nightCount;
    public static bool isNightActive = true;
    public TMP_Text time;
    public TMP_Text nightText;

    public WinScreen _win;
    private void Start()
    {
        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        /*var saveNight = new SaveNightData { nightCount = nightCount };
        var saveProfile = new SaveProfile<SaveNightData>("SaveNightData", saveNight);
        
        SaveManager.SaveAs(saveProfile, true);*/
        //Debug.Log(SaveManager.Load<SaveNightData>("SaveNightData").saveData.nightCount);
        isNightActive = true;
        //Debug.Log(SaveManager.Load<SaveNightData>("SaveNightData").saveData.nightCount);
        nightCount = (SaveManager.Load<SaveNightData>("SaveNightData").saveData.nightCount);

       
    }
    
    private void Update()
    {
        
        time.text = (int)nightDuration switch
        {
            60 => "1:00AM",
            120 => "2:00AM",
            180 => "3:00AM",
            240 => "4:00AM",
            300 => "5:00AM",
            360 => "6:00AM",
            _ => time.text
        };

        nightText.text = nightCount switch
        {
            1 => "Night 1",
            2 => "Night 2",
            3 => "Night 3",
            4 => "Night 4",
            5 => "Night 5",
            _ => nightText.text
        };
        
        /*if(Input.GetKeyDown(KeyCode.X))
            nightCount = (SaveManager.Load<SaveNightData>("SaveNightData").saveData.nightCount);*/
        
        if (!isNightActive) return;
        
        nightDuration += Time.deltaTime; //remove 10 later, just for testing purposes rn
        if (nightDuration >= 360)
            EndNight();
        
    }
    
    private void EndNight()
    {
        isNightActive = false;
        nightCount++;

        var saveNight = new SaveNightData { nightCount = nightCount };
        var saveProfile = new SaveProfile<SaveNightData>("SaveNightData", saveNight);
        
        SaveManager.SaveAs(saveProfile, true);
        
        _win.Win();
    }

    private bool ValidateReferences()
    {
        var isValid = true;

        if (time == null)
        {
            Debug.LogError($"{nameof(NightCycle)} is missing {nameof(time)} reference.", this);
            isValid = false;
        }

        if (nightText == null)
        {
            Debug.LogError($"{nameof(NightCycle)} is missing {nameof(nightText)} reference.", this);
            isValid = false;
        }

        if (_win == null)
        {
            Debug.LogError($"{nameof(NightCycle)} is missing {nameof(_win)} reference.", this);
            isValid = false;
        }

        return isValid;
    }
}
