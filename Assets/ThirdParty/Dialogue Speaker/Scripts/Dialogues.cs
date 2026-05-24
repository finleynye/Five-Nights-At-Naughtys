using UnityEngine;
using UnityEngine.Events;

[System.Serializable]

public class Dialogues
{
    //the audio source of the dialogue 
    [Tooltip("The actual AudioSource that will play")]
    public AudioClip clip;
    
    //when it's time to play wait (t) seconds before playing
    //used to add a little breathing room before the next dialogue
    [Tooltip("The amount of seconds before the audio plays.")]
    public float time;

    //the string text of the subtitle
    [Tooltip("The printed subtitle.")]
    public string subtitles;

    [Tooltip("A method to trigger when this audio plays. Optional and can be left empty.")]
    public UnityEvent triggerEvent;
}
