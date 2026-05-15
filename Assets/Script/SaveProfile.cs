using UnityEngine;

[System.Serializable]
public sealed class SaveProfile<T> where T : SaveProfileData
{
    public string name;
    public T saveData; //no need to define data type
    
    private SaveProfile() { } //used by serializer
        
    public SaveProfile(string name, T saveData)
    {
        this.name = name;
        this.saveData = saveData;
    }
}

public abstract record SaveProfileData { }

public record SaveNightData : SaveProfileData 
{
    public int nightCount; 
}

public record SaveSettingsData : SaveProfileData
{
    public bool subtitles;
    public bool fullscreen;
    public bool vsync;
    public bool motionBlur;
    public bool showFPS;

    public float sensitivity;
    public Vector2Int resolution;

    public float masterVolume;
}

public record SaveCutSceneData : SaveProfileData
{
    public bool newSave = true;
}

public record SaveTutorialData : SaveProfileData
{
    public bool hasPlayedTutorial;
} 


