using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [Header("NightDuration")]
    public NightCycle nightCycle;

    [Header("UI")]
    public GameObject winScreen;
    public GameObject playerUI;
    public GameObject pcUI;
    public GameObject cameraLayout;

    [Header("Audio")]
    public AudioSource yay;

    [Header("Animation")]
    private Animator anim;

    [Header("DemoShit")]
   [SerializeField] private bool isDemo;

    public bool forcedWin;
    void Start()
    {
        isDemo = true;
        anim = GetComponent<Animator>();
    }


    public void Win()
    {
        if (isDemo && nightCycle.nightCount is 3)
        {
            DemoComplete();
        }
        else
        {
            anim.SetTrigger("Win");
            playerUI.SetActive(false);
            pcUI.SetActive(false);
            cameraLayout.SetActive(false);
        }
    }

    public void DemoComplete()
    {
        anim.SetTrigger("CompleteGame");
        playerUI.SetActive(false);
        pcUI.SetActive(false);
        cameraLayout.SetActive(false);
        nightCycle.nightCount = 1;
        SaveManager.Delete("SaveNightData");
    }
    
    public void YayPlay()
        => yay.Play();
}
