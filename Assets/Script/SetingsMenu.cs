using System.Collections.Generic;
using TMPro;
using UnityEngine.Audio;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Resolutions")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private Resolution[] _resolutions;
    private int _currentResIndex;
    
    [Header("Toggles")] 
    public Toggle fullscreenToggle;
    public Toggle motionBlurToggle;
    public Toggle fpsToggle;
    public Toggle vsyncToggle;
    public Toggle subtitlesToggle;
    public float sensitivity;
    
    [Header("Audio")]
    [SerializeField] private AudioMixer masterMixer;
    public TMP_Text mastLabel, musicLabel, sfxLabel;
    public Slider masterSlider, musicSlider, sfxSlider;

    private void Start()
    {
        _resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        var resolutionOptions = new List<string>();
        _currentResIndex = 0;
        for (var i = 0; i < _resolutions.Length; i++)
        {
            var option = $"{_resolutions[i].width} x {_resolutions[i].height}";
            resolutionOptions.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width && 
                _resolutions[i].height == Screen.currentResolution.height)
                _currentResIndex = i;
        }


        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = _currentResIndex;
        resolutionDropdown.RefreshShownValue();



        float vol = -30f;
        masterMixer.GetFloat("MasterVolume", out vol);
        masterSlider.value = vol;
        mastLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();

        masterMixer.GetFloat("MusicVolume", out vol);
        musicSlider.value = vol;
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();

        masterMixer.GetFloat("SfxVolume", out vol);
        sfxSlider.value = vol;
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();

    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetSensitivity(float sensitivityValue)
        => sensitivity = sensitivityValue;
    
    public void ToggleVSync(bool toggle)
        => vsyncToggle.isOn = toggle;
    
    public void ToggleSubtitles(bool toggle)
        => subtitlesToggle.isOn = toggle;

    public void ToggleFullscreen(bool toggle)
    {
        
        fullscreenToggle.isOn = toggle;

    }
    
    public void ToggleFPS(bool toggle)
        => fpsToggle.isOn = toggle;
    
    public void ToggleMotionBlur(bool toggle)
        => motionBlurToggle.isOn = toggle;

    public void SetMasterVolume()
    {
        mastLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        masterMixer.SetFloat("MasterVolume", masterSlider.value);
    }
    public void SetMusicVolume()
    {
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        masterMixer.SetFloat("MusicVolume", musicSlider.value);
    }
    public void SetSFXVolume()
    {
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
        masterMixer.SetFloat("SfxVolume", sfxSlider.value);
    }

    public void SaveSettings()
    {
        var saveSettings = new SaveSettingsData()
        {
            fullscreen = fullscreenToggle.isOn,
            motionBlur = motionBlurToggle.isOn,
            resolution = new Vector2Int(Screen.currentResolution.width,
                Screen.currentResolution.height),
            
            sensitivity = sensitivity,
            showFPS = fpsToggle.isOn,
            subtitles = subtitlesToggle.isOn,
            vsync = vsyncToggle.isOn
        };
        
        var saveProfile = new SaveProfile<SaveSettingsData>("SaveSettingsData", saveSettings);
        
        SaveManager.SaveAs(saveProfile, true);
    }
    
    public void LoadSettings()
    {
        //subtitlesToggle.isOn = SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData.subtitles;
        Screen.fullScreen = (SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData.fullscreen);
        //vsyncToggle.isOn = SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData.vsync;
        //motionBlurToggle.isOn = SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData.motionBlur;
        //fpsToggle.isOn = SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData.showFPS;

        //sensitivity = SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData.sensitivity;
        /*Screen.SetResolution(SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData
            .resolution.x, SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData
            .resolution.y, SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData.fullscreen);*/
        
        Debug.Log($"is fullscreen?: {Screen.fullScreen}. Should be {SaveManager.Load<SaveSettingsData>("SaveSettingsData").saveData.fullscreen}");

    }
}