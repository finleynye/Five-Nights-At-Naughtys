using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DialogueSpeaker))]
public class DialogueSpeakerCustomInspector : Editor
{
    SerializedProperty centralAudio,
    playOnAwake,
    subtitlesText,
    useSubtitles,
    dialogues,
    fireFinishEvent,
    finishedEvent;


    void OnEnable() 
    {
        centralAudio = serializedObject.FindProperty("centralAudio");
        subtitlesText = serializedObject.FindProperty("subtitlesText");
        useSubtitles = serializedObject.FindProperty("useSubtitles");
        dialogues = serializedObject.FindProperty("dialogues");
        fireFinishEvent = serializedObject.FindProperty("fireFinishEvent");
        finishedEvent = serializedObject.FindProperty("finishedEvent");   
        playOnAwake = serializedObject.FindProperty("playOnAwake");
    }

    
    public override void OnInspectorGUI()
    {
        var button = GUILayout.Button("Click for more tools");
        if (button) Application.OpenURL("https://assetstore.unity.com/publishers/39163");
        EditorGUILayout.Space(5);

        DialogueSpeaker script = (DialogueSpeaker) target;

        EditorGUILayout.LabelField("Dialogue Options", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(centralAudio);
        EditorGUILayout.PropertyField(playOnAwake);
        EditorGUILayout.Space(5);
        EditorGUILayout.PropertyField(subtitlesText);
        EditorGUILayout.PropertyField(useSubtitles);
        EditorGUILayout.PropertyField(dialogues);
        
        
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Script Options", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(fireFinishEvent);
        if (script.fireFinishEvent) {
            EditorGUILayout.PropertyField(finishedEvent);
        }

        
        serializedObject.ApplyModifiedProperties();
    }
}
