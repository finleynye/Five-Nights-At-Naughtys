using UnityEngine;
using UnityEngine.Rendering;
using TMPro;

public class SettingsLoader : MonoBehaviour
{
    [SerializeField] private PlayerLook player;
    [SerializeField] private GameObject subtitlesObj;
    [SerializeField] private GameObject framerateObj;
    [SerializeField] private Volume motionBlur;

    private void Start()
    {
        //set sensitivity
        player.sensitivity = SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData.sensitivity;
        
        //toggle subtitles
        subtitlesObj.GetComponent<TMP_Text>().enabled =
            SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData.subtitles;
        
        //set fullscreen
        Screen.fullScreen = SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData.fullscreen;
        
        //motion blur
        motionBlur.profile.components[3].active =
            SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData.motionBlur;
        
        //show fps
        framerateObj.GetComponent<TMP_Text>().enabled =
            SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData.showFPS;
        
        //vsync
        //someone else pls do :thumbs_up:
    }
}
