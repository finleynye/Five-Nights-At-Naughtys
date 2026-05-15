using UnityEngine;

public class EndMethod : MonoBehaviour
{
    public DialogueSpeaker ds;
    public GameObject text;
    

    void Update()
    {
        // check if dialogue speaker has finished
        if (ds.isFinished) {
            text.SetActive(true);
        }
        else {
            text.SetActive(false);
        }
    }
}
