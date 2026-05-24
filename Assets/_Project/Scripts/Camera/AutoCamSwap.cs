using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCamSwap : MonoBehaviour
{
    public List<Camera> cameras = new List<Camera>();

    private int activeCameraIndex = 0;

    public AudioSource camSwap;

    public float swapTime = 5f;

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
        swapTime -= Time.deltaTime;

        if (swapTime <= 0)
        {
            foreach (var t in cameras)
                t.gameObject.SetActive(false);

            SwitchCameraForward();
            swapTime = 5f;
        }
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

    private void ActivateCamera(int index)
    {
        cameras[index].gameObject.SetActive(true);
    }

    private void DeactivateCamera(int index)
    {
        cameras[index].gameObject.SetActive(false);
    }
}
