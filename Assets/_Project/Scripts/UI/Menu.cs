using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Menu : MonoBehaviour
{
    public Animator anim;
    public float seconds;

    private void Start()
    {
        anim = GetComponent<Animator>();

        var cutSceneTrigger = new SaveCutSceneData { newSave = true };
        
        var saveProfile = new SaveProfile<SaveCutSceneData>("SaveCutSceneData", cutSceneTrigger);
        SaveManager.Save(saveProfile);
    }
   

    public void Play()
    {
        //check if it exists and is a new save OR if theres no file
        if (SaveManager.Load<SaveCutSceneData>("SaveCutSceneData").saveData.newSave)
            anim.SetTrigger("FadeOut");
        else SceneManager.LoadScene("MainScene");
    }

    public void LoadingScene()
    {
        SceneManager.LoadScene("CutScene");

        var cutSceneTrigger = new SaveCutSceneData()
        {
            newSave = false
        };

        var saveProfile = new SaveProfile<SaveCutSceneData>("SaveCutSceneData", cutSceneTrigger);
        SaveManager.SaveAs(saveProfile, true);
    }
}
