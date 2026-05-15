using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraSwap : MonoBehaviour{
    
    public List<Camera> cameras = new List<Camera>();
    public List<TMP_Text> camText = new List<TMP_Text>();

    private int activeCameraIndex = 0;

    public AudioSource camSwap;

    private void Start()
    {
        if (cameras.Count > 0)
        {
            ActivateCamera(activeCameraIndex);
        }
        else
        {
            Debug.LogWarning("No cameras assigned to the CameraSwitch script.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            SwitchCameraForward();
        if(Input.GetKeyDown(KeyCode.Q))
            SwitchCameraBackwards();
        
        Debug.Log(activeCameraIndex);
    }

    private void SwitchCameraForward()
    {
        DeactivateCamera(activeCameraIndex);
        camSwap.Play();
        
        //activeCameraIndex = (activeCameraIndex + 1) % cameras.Count;
        activeCameraIndex = Mathf.Clamp(activeCameraIndex + 1, 0, cameras.Count + 1);
        if (activeCameraIndex is 6)
            activeCameraIndex = 0;

        ActivateCamera(activeCameraIndex);
    }
    private void SwitchCameraBackwards()
    {
        DeactivateCamera(activeCameraIndex);
        camSwap.Play();
        
        activeCameraIndex = Mathf.Clamp(activeCameraIndex - 1, -1, cameras.Count - 1);
        if (activeCameraIndex is -1)
            activeCameraIndex = 5;

        ActivateCamera(activeCameraIndex);
    }

    private void ActivateCamera(int index)
    {
        cameras[index].gameObject.SetActive(true);
        camText[index].color = Color.green;
    }

    private void DeactivateCamera(int index)
    {
        cameras[index].gameObject.SetActive(false);
        camText[index].color = Color.white;
    }

    public void ForceCam(int index)
    {
        activeCameraIndex = index;
        foreach (var t in cameras)
            t.gameObject.SetActive(false);
        
        foreach(var c in camText)
            c.color = Color.white;
        
        
        cameras[index].gameObject.SetActive(true);
        camText[index].color = Color.green;
    }
}
