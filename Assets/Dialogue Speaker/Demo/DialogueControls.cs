using UnityEngine;

public class DialogueControls : MonoBehaviour
{
    // get the Dialogue Speaker component
    public DialogueSpeaker ds;


    void Update()
    {   
        // start the dialogue on pressing SPACE
        if (Input.GetKeyDown(KeyCode.Space)){
            ds.Play();
        }
        
        // skip current dialogue and move on to the next one on pressing E
        if (Input.GetKeyDown(KeyCode.E)){
            ds.Skip();
        }

        // pause current dialogue on pressing D
        if (Input.GetKeyDown(KeyCode.D)){
            ds.Pause();
        }
        
        // resume the stopped dialogue on pressing F
        if (Input.GetKeyDown(KeyCode.F)){
            ds.Resume();
        }
    }
}
