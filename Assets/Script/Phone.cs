using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    [Header("Phone Attributes")]
    public int equip;

    [Header("HeartGameObject")]
    public GameObject Heart;

    [Header("Audio")]
    public AudioSource Equipped;

    [Header("Animations")]
    public Animator anim;

    public GameObject instructions;
    public bool hasLoadedTutorial;


    private void Start()
    {
        if(SaveManager.Load<SaveTutorialData>("SaveTutorialData").saveData.hasPlayedTutorial)
            Destroy(instructions);
    }

    void Update()
    {
        if (PC.isOn) return;
        Equip();
        UnEquip();
    }
    public void Equip()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (instructions is not null)
            {
                Destroy(instructions);
            }
            equip++;
            anim.SetInteger("EquipNum", equip);
            Heart.SetActive(true);
            Equipped.Play();
        }

    }
    public void UnEquip()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (equip >= 2)
            {
                equip = 0;
                anim.SetInteger("EquipNum", equip);
                Heart.SetActive(false);
            }
        }
    }
}
