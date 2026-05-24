using System.Collections;
using System.Collections.Generic;
using GLTFast.Schema;
using UnityEngine;

public class PC : MonoBehaviour
{
    [SerializeField] private Animator pcCanvas;
    [SerializeField] private Animator playerCanvas;
    [SerializeField] private GameObject pcCanvasObj;
    [SerializeField] private NightCycle _nightCycle;
    
    [Header("Crashing")]
    [SerializeField] private GameObject pcParentObj; //This is for the crash system
    [SerializeField] private GameObject cameras; //This is for the crash system
    [SerializeField] private GameObject camVolume; //This is finnix code
    [SerializeField] private GameObject playerCameras; //This is for the crash system
    [SerializeField] private GameObject PCCameras; //This is for the crash system
    [SerializeField] private GameObject exitCamsButton; //This is for the crash system
    [SerializeField] private GameObject layoutCameras;
    public float crashTimer = 180f; // 180 seconds = 3 Minutes
    public float currentTimer; //This is kaden ellis code
    public bool isCrashed; //This is kaden ellis code
    public int crashPercent; //This is kaden ellis code
    public int douglasManipulation = 1; //This is swagward code
    
    [Header("Errors")]
    [SerializeField] private GameObject ErrorScreen; //This is kaden ellis code
    [SerializeField] private GameObject ErrorScreen2;//This is kaden ellis code
    [SerializeField] private GameObject MonitorScreen; //This is kaden ellis code
    [SerializeField] private GameObject MonitorScreen2; //This is kaden ellis code
    [SerializeField] private AudioSource ErrorSound; //This is kaden ellis code
    [SerializeField] private AudioSource rebootSound; //This is kaden ellis code

    [Header("Opening PC")] 
    public LayerMask pc; //This is kaden ellis code
    public Transform eyes; //This is kaden ellis code
    public bool canOpen; //This is kaden ellis code
    public int range; //This is kaden ellis code
    public static bool isOn; //This is kaden ellis code
    public bool camsOn; //This is kaden ellis code
    public int input; //This is kaden ellis code

    public GameObject instructions; //This is finnix code
    void Start()
    {
        if (!ValidateReferences())
        {
            enabled = false;
            return;
        }

        pcCanvasObj.SetActive(false);
        currentTimer = crashTimer;
    }

    void Update()
    {
        if (!NightCycle.isNightActive)
            return;
        
        LoadPC();
        ShutDownPC();
        
        if(isOn)
            HandleCrash();
        
        if(SaveManager.Load<SaveTutorialData>("SaveTutorialData").saveData.hasPlayedTutorial)
            Destroy(instructions);

        if (_nightCycle.nightDuration >= 360)
        {
            layoutCameras.SetActive(false);
            exitCamsButton.SetActive(false);
            pcCanvasObj.SetActive(false);
        }
    }
    
    private void LoadPC()
    {
        canOpen = Physics.Raycast(eyes.position, eyes.forward, range, pc);
        
        if (Input.GetKeyDown(KeyCode.F) && !isCrashed && canOpen)
        {
            if (instructions is not null)
            {
                Destroy(instructions);
            }
            input++;
            isOn = true;
            pcCanvasObj.SetActive(true);
            pcParentObj.SetActive(true);
            exitCamsButton.SetActive(false);
            pcCanvas.SetTrigger("FadeIn");
            Cursor.lockState = CursorLockMode.Confined;
        }

    }
    private void ShutDownPC()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (input >= 2)
            {
                isOn = false;
                pcCanvas.SetTrigger("FadeOut");
                pcCanvasObj.SetActive(false);
                cameras.SetActive(false);
                camVolume.SetActive(false);
                playerCameras.SetActive(true);
                layoutCameras.SetActive(false);
                exitCamsButton.SetActive(false);
                input = 0;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

    }

    private void HandleCrash()
    {
        currentTimer -= Time.deltaTime * douglasManipulation;

        if (currentTimer <= 0)
        {
            //Debug.Log("Attempted crash");
            if (!isCrashed && Random.Range(1, crashPercent) == 1)
            {
                //crash text popup
                CrashPC();
                //Debug.Log("Crash");
            }

            currentTimer = crashTimer;
            //Debug.Log("Crash failed");
        }
    }

    private void CrashPC()
    {
        isCrashed = true;
        isOn = false;
        
        pcParentObj.SetActive(false);
        cameras.SetActive(false);
        camVolume.SetActive(false);
        exitCamsButton.SetActive(false);
        layoutCameras.SetActive(false);
        playerCameras.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        input = 0;
        
        ErrorSound.Play();
        Invoke(nameof(ShowError),.01f);

        StartCoroutine(RecoverAfterCrash());
    }

    private IEnumerator RecoverAfterCrash()
    {
        yield return new WaitForSeconds(10f);
        rebootSound.Play();
        //pcParentObj.SetActive(true);
        isCrashed = false;
        //isOn = true;
        HideError();
        Cursor.lockState = CursorLockMode.Locked;
        //maybe play audio source to show its fixed?
        yield return null;
    }

    private void ShowError()
    {
        ErrorScreen.SetActive(true);
        ErrorScreen2.SetActive(true);
        MonitorScreen.SetActive(false);
        MonitorScreen2.SetActive(false);
    }

    private void HideError()
    {
        ErrorScreen.SetActive(false);
        ErrorScreen2.SetActive(false);
        MonitorScreen.SetActive(true);
        MonitorScreen2.SetActive(true);
    }

    private bool ValidateReferences()
    {
        var isValid = true;

        if (pcCanvas == null)
        {
            Debug.LogError($"{nameof(PC)} is missing {nameof(pcCanvas)} reference.", this);
            isValid = false;
        }

        if (playerCanvas == null)
        {
            Debug.LogError($"{nameof(PC)} is missing {nameof(playerCanvas)} reference.", this);
            isValid = false;
        }

        if (pcCanvasObj == null)
        {
            Debug.LogError($"{nameof(PC)} is missing {nameof(pcCanvasObj)} reference.", this);
            isValid = false;
        }

        if (_nightCycle == null)
        {
            Debug.LogError($"{nameof(PC)} is missing {nameof(_nightCycle)} reference.", this);
            isValid = false;
        }

        if (eyes == null)
        {
            Debug.LogError($"{nameof(PC)} is missing {nameof(eyes)} reference.", this);
            isValid = false;
        }

        return isValid;
    }
}
