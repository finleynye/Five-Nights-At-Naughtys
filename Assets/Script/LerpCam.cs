using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpCam : MonoBehaviour
{
    public float[] FOVS = { 50, 35 };
    public float speed = 2f;
    public bool zoomIn, zoomOut;
    Camera myCamera;
    int FOVpos = 0;

    [Header("Restrictions")]
    [SerializeField] private PlayerLook playerlook;
    
    void Start()
    {


        myCamera = GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {

        if (!NightCycle.isNightActive)
            return;

        if (Input.GetMouseButtonDown(1) && playerlook.right || Input.GetMouseButtonDown(1) && playerlook.left || Input.GetMouseButtonDown(1) && playerlook.back)
            ZoomIn();
        if (Input.GetMouseButtonUp(1) && playerlook.right || Input.GetMouseButtonUp(1) && playerlook.left || Input.GetMouseButtonUp(1) && playerlook.back)
            ZoomOut();



        if (zoomIn)
        {
            myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, FOVS[FOVpos], speed * Time.deltaTime);
            if (Mathf.Approximately(myCamera.fieldOfView, FOVS[FOVpos]))
            {
                zoomIn = false;
            }
        }
        if (zoomOut)
        {
            myCamera.fieldOfView = Mathf.Lerp(myCamera.fieldOfView, FOVS[FOVpos], speed * Time.deltaTime);
            if (Mathf.Approximately(myCamera.fieldOfView, FOVS[FOVpos]))
            {
                zoomOut = false;
            }
        }
    }

    void ZoomIn()
    {
        if (FOVpos >= FOVS.Length - 1)
        {
            FOVpos = FOVS.Length - 1;
        }
        else
        {
            FOVpos += 1;
        }
        zoomIn = true;
    }
    void ZoomOut()
    {
        if (FOVpos <= 0)
        {
            FOVpos = 0;
        }
        else
        {
            FOVpos -= 1;
        }
        zoomOut = true;
    }
}
