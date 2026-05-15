using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpScare : MonoBehaviour
{
    [Header("Player Variables")]
    public PlayerLook playerCam;
    public GameObject FlashLight;
    public GameObject Phone;

    [Header("Douglas Variables")]
    public AudioSource scareNoise;
    public Animator ScareDoug;

    [Header("Player")]
    public GameObject playerUI;

    [Header("UI")]
    public Animator fadeIn;
    public GameObject pcUI;

    [Header("Rendering")]
    public LayerMask douglas;

    [Header("PC")]
    public PC pc;

    public void TriggerJumpscare()
    {
        NightCycle.isNightActive = false;
        pcUI.SetActive(false);
        pc.enabled = false;
        Camera.main.cullingMask = douglas;
        playerCam.enabled = false;
        ScareDoug.SetTrigger("Jumpscare");
        playerUI.SetActive(false);
        Phone.SetActive(false);
        FlashLight.SetActive(false);
       
        
    }

    public void Jumpscare()
        => scareNoise.Play();


    public void LoadLose()
    {
        fadeIn.SetTrigger("Fade");
    }


}