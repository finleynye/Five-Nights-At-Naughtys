using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Animator anim;
    public bool isBack;
    public Transform player;
    public PlayerLook pluhLook;
    public GameObject instructions;
    // Update is called once per frame
    private void Start()
    {
        if(SaveManager.Load<SaveTutorialData>("SaveTutorialData").saveData.hasPlayedTutorial)
            Destroy(instructions);
    }
    
    void Update()
    {
        if (PC.isOn) return;
        if (!NightCycle.isNightActive) return;
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (instructions is not null)
            {
                Destroy(instructions);
                
                var saveTutorialData = new SaveTutorialData() { hasPlayedTutorial = true };
                var saveProfile = new SaveProfile<SaveTutorialData>("SaveTutorialData", saveTutorialData);
        
                SaveManager.SaveAs(saveProfile, true);
            }
            pluhLook.left = false;
            anim.SetBool("Idle", true);
            anim.SetBool("Back", false);
            anim.SetBool("Left", false);
            anim.SetBool("Right", false);
        }
       
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (instructions is not null)
            {
                Destroy(instructions);
                
                var saveTutorialData = new SaveTutorialData() { hasPlayedTutorial = true };
                var saveProfile = new SaveProfile<SaveTutorialData>("SaveTutorialData", saveTutorialData);
        
                SaveManager.SaveAs(saveProfile, true);
            }
            anim.SetBool("Left", true);
            anim.SetBool("Right", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Back", false);
            pluhLook.left = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (instructions is not null)
            {
                Destroy(instructions);
                
                var saveTutorialData = new SaveTutorialData() { hasPlayedTutorial = true };
                var saveProfile = new SaveProfile<SaveTutorialData>("SaveTutorialData", saveTutorialData);
        
                SaveManager.SaveAs(saveProfile, true);
            }
            pluhLook.left = false;
            anim.SetBool("Right", true);
            anim.SetBool("Left", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Back", false);
        }

        Backwards();
    }

    public void Backwards()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (instructions is not null)
            {
                Destroy(instructions);
                
                var saveTutorialData = new SaveTutorialData() { hasPlayedTutorial = true };
                var saveProfile = new SaveProfile<SaveTutorialData>("SaveTutorialData", saveTutorialData);
        
                SaveManager.SaveAs(saveProfile, true);
            }
            anim.SetBool("Back", true);
            anim.SetBool("Left", false);
            anim.SetBool("Idle", false);
            anim.SetBool("Right", false);
            pluhLook.left = false;
        }
    }


    public void idle()
    {
        isBack = false;
    }

}
